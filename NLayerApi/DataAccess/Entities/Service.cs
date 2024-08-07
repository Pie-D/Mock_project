using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Service:Audit
    {
        [Key]
        public int ServiceId { get; set; }
        [MaxLength(300)]
        public string ServiceName { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public Contact Contact { get; set; }

        // Many-to-many navigation property
        public ICollection<OrganisationService> OrganisationServices { get; set; }
        public ICollection<Premise> Premises { get; set; }

    }
}
