using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Contact:Audit
    {
        [Key]
        public int ContactId { get; set; } // Primary Key
        [MaxLength(300)]
        public string FirstName { get; set; }
        [MaxLength(300)]
        public string Surname { get; set; }
        [MaxLength(300)]
        public string KnownAs { get; set; }
        [MaxLength(300)]
        public string OfficePhone { get; set; }
        [MaxLength(300)]
        public string MobilePhone { get; set; }
        [MaxLength(300)]
        public string StHomePhone { get; set; }
        [MaxLength(300)]
        public string Email { get; set; }
        [MaxLength(300)]
        public string? ManagerName { get; set; }
        [MaxLength(300)]
        public string ContactType { get; set; }
        [MaxLength(300)]
        public string BestContactMethod { get; set; }
        [MaxLength(300)]
        public string JobRole { get; set; }
        [MaxLength(300)]
        public string WorkBase { get; set; }
        [MaxLength(300)]
        public string JobTitle { get; set; }
    public bool IsActive { get; set; }
    public int? ManagerId { get; set; } // Foreign Key (self-referencing)

    // Navigation properties
    public Contact Manager { get; set; }
    public ICollection<Contact> Subordinates { get; set; }
    public ICollection<Organisation> Organisations { get; set; }
    public ICollection<Directorate> Directorates { get; set; }
    public ICollection<Department> Departments { get; set; }
    public ICollection<Team> Teams { get; set; }
    public ICollection<Programme> Programmes { get; set; }
    public ICollection<Service> Services { get; set; }
}

}
