using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWeb.Dto
{
    public class CreateOrganisationDto
    {
        public int OrganisationId { get; set; } // Primary Key
        [Required]
        public string OrgName { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        public string? LeadContact { get; set; }
        public string? PreferredOrganisation { get; set; }
        public string? ExpressionOfInternet { get; set; }
        public AddressDto Address { get; set; }
        public CompanyContactDto CompanyContact { get; set; }
        public List<int> ReferenceDataIds { get; set; }
    }
}
