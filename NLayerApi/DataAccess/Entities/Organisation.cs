using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.Entities
{
    public class Organisation:Audit
    {
        [Key]
        public int OrganisationId { get; set; } // Primary Key
        [Required]
        [MaxLength(300)]
        public string OrgName { get; set; }
        [Required]
        [MaxLength(300)]
        public string ShortDescription { get; set; }
        [MaxLength(300)]
        public string? LeadContact { get; set; }
        [MaxLength(300)]
        public string? PreferredOrganisation { get; set; }
        [MaxLength(300)]
        public string? ExpressionOfInternet { get; set; }


        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        [Required]
        public int CompanyContactId { get; set; }
        public CompanyContact CompanyContact { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(300)]
        public string? TrustRegionName { get; set; }
        [MaxLength(300)]
        public string? TrustDistrictName { get; set; }
        [MaxLength(300)]
        public string? GovOfficeRegionName { get; set; }
        public ICollection<SupportingMaterial> SupportingMaterials { get; set; }
        public ICollection<Directorate> Directorates { get; set; }
        public ICollection<OrganisationProgramme> OrganisationProgrammes { get; set; }
        public ICollection<OrganisationService> OrganisationServices { get; set; }
        public ICollection<OrganisationReferenceData> OrganisationReferenceDatas { get; set; }
    }

}
