using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Directorate:Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DirectorateId { get; set; } // Primary Key
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string? ShortDescription { get; set; }
        [MaxLength(300)]
        public string? LeadContact { get; set; }
        public bool IsActive { get; set; }



        public int OrganisationId { get; set; } // Foreign Key
        public int ContactId { get; set; } // Foreign Key

        // Navigation properties
        [ForeignKey("OrganisationId")]
        public Organisation Organisation { get; set; }

        [ForeignKey("ContactId")]
        public Contact Contact { get; set; }
        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        [ForeignKey("CompanyContactId")]
        public CompanyContact CompanyContact { get; set; }
        public int CompanyContactId { get; set; }
        public ICollection<Department> Departments { get; set; }
    }

}
