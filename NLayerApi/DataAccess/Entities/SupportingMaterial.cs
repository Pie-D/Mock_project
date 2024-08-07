using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class SupportingMaterial:Audit
    {
        [Key]
        public int MaterialId { get; set; }
        // public int OrganisationId { get; set; }
        [Required]
        [MaxLength(300)]
        public string Url { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        [MaxLength(300)]
        public string Type { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
    }
}
