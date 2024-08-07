using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class TrustRegion:Audit
    {
        public int TrustRegionId { get; set; }
        [MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public int NationId { get; set; }
        public Nation Nation { get; set; }
        public ICollection<TrustDistrict> TrustDistricts { get; set; }
    }
}
