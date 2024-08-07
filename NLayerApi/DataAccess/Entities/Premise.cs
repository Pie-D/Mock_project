using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Premise:Audit
    {
        [Key]
        public int PremiseId { get; set; } // Primary Key
        [Required]
        [MaxLength(300)]
        public string LocationName { get; set; }
        [MaxLength(300)]
        public string KnowAs { get; set; }
        [MaxLength(300)]
        public string LocationOrganisation { get; set; }
        [Required]
        [MaxLength(300)]
        public string LocationStatus { get; set; }
        public DateTime LocationStatusDate { get; set; }
        public bool PrimaryLocation { get; set; }
        public bool LocationManage { get; set; }
        public bool STNetwork { get; set; }
        [MaxLength(300)]
        public string LocationType { get; set; }
        [MaxLength(300)]
        public string LocationDescription { get; set; }
        public bool NewShop { get; set; }
        public DateTime FlagDate { get; set; }

        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        [Required]
        public int CompanyContactId { get; set; }
        public CompanyContact CompanyContact { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; } // Foreign Key
    }
}
