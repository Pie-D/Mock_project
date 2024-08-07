using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class County:Audit
    {
        public int CountyId { get; set; }
        [MaxLength(300)]
        public string CountyName { get; set; }
        public int NationId { get; set; }
        public Nation Nation { get; set; }
        public ICollection<Town> Towns { get; set; }
        public ICollection<GovOfficeRegion> GovOfficeRegions { get; set; }
    }
}
