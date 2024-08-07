using CommonWeb.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWeb.Models
{
    public class CreateOrganisationModel
    {
        public int AddressId { get; set; }
        public int ContactId { get; set; }
        public int CompanyContactId { get; set; }
        public CreateOrganisationDto CreateOrganisationDto { get; set; }
        public string CreatedBy { get; set; }
    }
}
