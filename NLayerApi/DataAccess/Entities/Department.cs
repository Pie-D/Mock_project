using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Department:Audit
    {
    [Key]
    public int DepartmentId { get; set; } // Primary Key
    public int DirectorateId { get; set; } // Foreign Key
    public int ContactId { get; set; } // Foreign Key
    [Required]
        [MaxLength(300)]
        public string Name { get; set; }
    [Required]
        [MaxLength(300)]
        public string ShortDescription { get; set; }
        [MaxLength(300)]
        public string? LeadContact { get; set; }
    public bool IsActive { get; set; }


    public CompanyContact CompanyContact { get; set; }
        public int? CompanyContactId { get; set; }
        public Address Address { get; set; }
    public int? AddressId { get; set; }
    public Directorate Directorate { get; set; }
    public Contact Contact { get; set; }
    public ICollection<Team> Teams { get; set; }
}
}
