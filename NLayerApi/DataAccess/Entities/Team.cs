using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Team:Audit
    {
        [Key]
        public int TeamId { get; set; }
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string? ShortDescription { get; set; }
        [MaxLength(300)]
        public string? LeadContact { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("ContactId")]
        public Contact Contact {get;set;}
        public int? ContactId {get;set;}
        public Address Address { get; set; }
        public int? AddressId {get;set;}
        public CompanyContact CompanyContact { get; set; }
    }
}
