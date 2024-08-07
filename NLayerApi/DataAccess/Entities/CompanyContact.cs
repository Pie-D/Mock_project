using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class CompanyContact:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyContactId { get; set;}
        [Required]
        [MaxLength(300)]
        public string PhoneNumber { get; set; }
        [MaxLength(300)]
        public string Fax { get; set; }
        [MaxLength(300)]
        public string Email { get; set; }
        [MaxLength(300)]
        public string WebAddress { get; set; }
        [MaxLength(300)]
        public string CharityNumber { get; set; }
        [MaxLength(300)]
        public string CompanyNumber { get; set; }
        [Required]
        [MaxLength(300)]
        public string TypeOfBusiness { get; set; }
        [MaxLength(300)]
        public string SICCode { get; set; }
        [MaxLength(300)]
        public string FullDescription { get; set; }
        public ICollection<Organisation> Organisations { get; set; }
        public ICollection<Directorate> Directorates { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<Premise> Premises { get; set; }


    }
}