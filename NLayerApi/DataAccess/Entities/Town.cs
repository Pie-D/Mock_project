using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Town:Audit
    {
        public int TownId { get; set; }
        public int CountyId { get; set; }
        public County County { get; set; }
        [MaxLength(300)]
        public string TownName { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
