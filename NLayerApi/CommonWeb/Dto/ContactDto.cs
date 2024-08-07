using System.ComponentModel.DataAnnotations;

namespace CommonWeb.Dto
{
    public class ContactDto
    {
        public int ContactId { get; set; } // Primary Key
        [MaxLength(300)]
        public string FirstName { get; set; }
        [MaxLength(300)]
        public string Surname { get; set; }
        [MaxLength(300)]
        public string KnownAs { get; set; }
        [MaxLength(300)]
        public string OfficePhone { get; set; }
        [MaxLength(300)]
        public string MobilePhone { get; set; }
        [MaxLength(300)]
        public string StHomePhone { get; set; }
        [MaxLength(300)]
        public string Email { get; set; }
        [MaxLength(300)]
        public string? ManagerName { get; set; }
        [MaxLength(300)]
        public string ContactType { get; set; }
        [MaxLength(300)]
        public string BestContactMethod { get; set; }
        [MaxLength(300)]
        public string JobRole { get; set; }
        [MaxLength(300)]
        public string WorkBase { get; set; }
        [MaxLength(300)]
        public string JobTitle { get; set; }
    public bool IsActive { get; set; }
    }
}