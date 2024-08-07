using Common.Dto;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IOrganisationServiceService
    {
        Task<IEnumerable<OrganisationServiceDto>?> CreateOrganisationService(CreateOrganisationServiceModel organisationService, string createdBy);
    }
}
