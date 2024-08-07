using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class TrustDistrict:Audit
    {
        [Key]
        public int TrustDistricId { get; set; }
        [MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public int TrustRegionId { get; set; }
        public TrustRegion TrustRegion { get; set; }
    }
}
