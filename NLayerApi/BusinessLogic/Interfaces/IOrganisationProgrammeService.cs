using Common.Dto;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IOrganisationProgrammeService
    {
        Task<IEnumerable<OrganisationProgrammeDto>?> CreateOrganisationProgramme(CreateOrganisationProgrammeModel createOrganisationProgramme, string createdBy);
    }
}
