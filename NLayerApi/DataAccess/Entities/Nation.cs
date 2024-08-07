using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Nation:Audit
    {
        public int NationId { get; set; }
        [MaxLength(300)]
        public string NationName { get; set; }
        public ICollection<County> Counties { get; set; }
        public ICollection<TrustRegion> TrustRegions { get; set; }
    }
}
