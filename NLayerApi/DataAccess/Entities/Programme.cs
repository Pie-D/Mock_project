using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Programme:Audit
    {
        [Key]
        public int ProgrammeId { get; set; }
        [MaxLength(300)]
        public string ProgrammeName { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public Contact Contact { get; set; }

        // Many-to-many navigation property
        public ICollection<OrganisationProgramme> OrganisationProgrammes { get; set; }
    }
}
