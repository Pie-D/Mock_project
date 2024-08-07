using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonWeb.Dto
{
    public class OrganisationDto
    {
        public int OrganisationId { get; set; } // Primary Key
        public string OrgName { get; set; }
        public string ShortDescription { get; set; }
        public string? LeadContact { get; set; }
        public string? PreferredOrganisation { get; set; }
        public string? ExpressionOfInternet { get; set; }
        public AddressDto Address { get; set; }
        public ContactDto Contact { get; set; }
        public string? TrustRegionName { get; set; }
        public string? TrustDistrictName { get; set; }
        public string? GovOfficeRegionName { get; set; }
    }
}
