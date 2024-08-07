using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWeb.Dto
{
    public class CreateDirectorateDto
    {
        public int DirectorateId { get; set; } // Primary Key
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? LeadContact { get; set; }
        public AddressDto Address { get; set; }
        public CompanyContactDto CompanyContact { get; set; }
        public string? GetAddressFrom { get; set; }
    }
}
