using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class GovOfficeRegion:Audit
    {
        public int GovOfficeRegionId { get; set; }
        [MaxLength(300)]
        public string GovOfficeReggionName { get; set; }
        public int CountyId { get; set; }
        public County County { get; set; }
    }
}
