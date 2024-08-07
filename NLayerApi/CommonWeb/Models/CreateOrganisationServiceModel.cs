using Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CreateOrganisationServiceModel
    {
        public IEnumerable<OrganisationServiceDto> OrganisationServicesDto { get; set; }
    }
}
