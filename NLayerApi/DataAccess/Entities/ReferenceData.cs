
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class ReferenceData:Audit
    {
        [Key]
        public int RefId { get; set; }
        [MaxLength(300)]
        public string RefCode { get; set; }
        [MaxLength(300)]
        public string RefValue { get; set; }
        public ICollection<OrganisationReferenceData> OrganisationReferenceDatas { get; set; }
    }
}