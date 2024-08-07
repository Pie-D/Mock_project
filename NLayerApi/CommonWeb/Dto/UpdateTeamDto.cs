using CommonWeb.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class UpdateTeamDto
    {
        public int TeamId { get; set; } // Primary Key
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? LeadContact { get; set; }
        public AddressDto Address { get; set; }
        public CompanyContactDto CompanyContact { get; set; }
        public string? GetAddressFrom { get; set; }
        public bool IsActive { get; set; }
    }
}
