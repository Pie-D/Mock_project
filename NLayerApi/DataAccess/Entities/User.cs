using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class User:IdentityUser<int>
    {
        [Key]
        //public int Id { get; set; }
        // public string UserName { get; set; }
        // public string Email { get; set; }
        // // public string Role { get; set; }
        // public string Password { get; set; }
        public ICollection<SupportingMaterial> SupportingMaterials { get; set; }
    }
}