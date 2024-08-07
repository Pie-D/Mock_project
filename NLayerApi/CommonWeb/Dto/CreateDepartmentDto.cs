using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWeb.Dto
{
    public class CreateDepartmentDto
    {
        public int DepartmentId { get; set; } // Primary Key
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        public string? LeadContact { get; set; }
        public AddressDto Address { get; set; }
        public CompanyContactDto CompanyContact { get; set; }
        public string? GetAddressFrom { get; set; }
    }
}
