namespace DataAccess.Entities
{
    public class OrganisationService:Audit
    {
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
