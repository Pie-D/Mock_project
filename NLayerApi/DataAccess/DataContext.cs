using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class DataContext : IdentityDbContext<User, Role, int>
{
   
    public DataContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<CompanyContact> CompanyContacts { get; set; }
    public DbSet<SupportingMaterial> SupportingMaterials { get; set; }
    public DbSet<Directorate> Directorates { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Programme> Programmes { get; set; }
    public DbSet<Premise> Premises { get; set; }
    public DbSet<OrganisationService> OrganisationServices { get; set; }
    public DbSet<OrganisationProgramme> OrganisationProgrammes { get; set; }
    public DbSet<Nation> Nations { get; set; }
    public DbSet<County> Counties { get; set; }
    public DbSet<Town> Towns { get; set; }
    public DbSet<TrustDistrict> TrustDistricts { get; set; }
    public DbSet<TrustRegion> TrustRegions { get; set; }
    public DbSet<ReferenceData> ReferenceDatas { get; set; }
    public DbSet<GovOfficeRegion> GovOfficeRegions { get; set; }

    public DbSet<OrganisationReferenceData> OrganisationReferenceDatas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OrganisationService>()
            .HasKey(op => new { op.OrganisationId, op.ServiceId });
        // Configure many-to-many relationships
        modelBuilder.Entity<OrganisationService>()
            .HasOne(os => os.Organisation)
            .WithMany(o => o.OrganisationServices)
            .HasForeignKey(os => os.OrganisationId)
            .OnDelete(DeleteBehavior.Cascade); // Or use DeleteBehavior.Restrict if it suits your design better

        modelBuilder.Entity<OrganisationService>()
            .HasOne(os => os.Service)
            .WithMany(s => s.OrganisationServices)
            .HasForeignKey(os => os.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrganisationProgramme>()
            .HasKey(op => new { op.OrganisationId, op.ProgrammeId });

        modelBuilder.Entity<OrganisationProgramme>()
            .HasOne(op => op.Organisation)
            .WithMany(o => o.OrganisationProgrammes)
            .HasForeignKey(op => op.OrganisationId)
            .OnDelete(DeleteBehavior.Restrict); // No cascading delete from Organisation

        modelBuilder.Entity<OrganisationProgramme>()
            .HasOne(op => op.Programme)
            .WithMany(p => p.OrganisationProgrammes)
            .HasForeignKey(op => op.ProgrammeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contact>()
            .HasOne(c => c.Manager)
            .WithMany(c => c.Subordinates)
            .HasForeignKey(c => c.ManagerId);


        modelBuilder.Entity<Department>()
            .HasKey(op => new { op.ContactId, op.DirectorateId });

        modelBuilder.Entity<Department>()
            .HasOne(d => d.Contact)
            .WithMany(c => c.Departments)
            .HasForeignKey(d => d.ContactId)
            .OnDelete(DeleteBehavior.Cascade); // Keep cascade for Contact

        modelBuilder.Entity<Department>()
            .HasOne(d => d.Directorate)
            .WithMany(c => c.Departments)
            .HasForeignKey(d => d.DirectorateId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Department>()
            .HasKey(d => d.DepartmentId);

        modelBuilder.Entity<Team>()
        .HasOne(t => t.Department)
        .WithMany(d => d.Teams)
        .HasForeignKey(t => t.DepartmentId)
        .OnDelete(DeleteBehavior.Restrict);

        // Configure relationship between Team and Contact
        modelBuilder.Entity<Team>()
        .HasOne(t => t.Contact)
        .WithMany(c => c.Teams)
        .HasForeignKey(t => t.ContactId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Directorate>()
            .HasOne(d => d.Organisation)
            .WithMany(o => o.Directorates)
            .HasForeignKey(d => d.OrganisationId)
            .OnDelete(DeleteBehavior.Restrict); // No cascading delete

        modelBuilder.Entity<Directorate>()
            .HasOne(d => d.Contact)
            .WithMany(c => c.Directorates)
            .HasForeignKey(d => d.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Organisation>()
            .HasIndex(o => o.OrgName)
            .IsUnique();

        modelBuilder.Entity<Directorate>()
            .HasIndex(o => o.Name)
            .IsUnique();

        modelBuilder.Entity<Department>()
            .HasIndex(o => o.Name)
            .IsUnique();

        modelBuilder.Entity<Team>()
            .HasIndex(o => o.Name)
            .IsUnique();

        modelBuilder.Entity<Premise>()
            .HasIndex(o => o.LocationName)
            .IsUnique();

        modelBuilder.Entity<SupportingMaterial>()
            .HasOne(a => a.User)
            .WithMany(u => u.SupportingMaterials)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        //modelBuilder.Entity<User>().HasNoKey();
        //modelBuilder.Entity<User>().HasNoKey();


        modelBuilder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN" }
            );
        modelBuilder.Entity<OrganisationReferenceData>()
            .HasKey(or => new { or.OrganisationId, or.RefId });

        modelBuilder.Entity<OrganisationReferenceData>()
            .HasOne(or => or.Organisation)
            .WithMany(o => o.OrganisationReferenceDatas)
            .HasForeignKey(or => or.OrganisationId);

        modelBuilder.Entity<OrganisationReferenceData>()
            .HasOne(or => or.ReferenceData)
            .WithMany(rd => rd.OrganisationReferenceDatas)
            .HasForeignKey(or => or.RefId);
    }


}
