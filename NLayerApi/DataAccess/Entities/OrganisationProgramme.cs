namespace DataAccess.Entities
{
    public class OrganisationProgramme:Audit
    {
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }

        public int ProgrammeId { get; set; }
        public Programme Programme { get; set; }
    }
}
