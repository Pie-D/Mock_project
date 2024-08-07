
using Common.Helper;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonWeb.Models;
using CommonWeb.Dto;

namespace BusinessLayer.Interfaces
{
    public interface IOrganisationService
    {
        Task<PagedList<OrganisationDto>> GetOrganisationAsync(OrganisationParams organisationParams);
        Task<bool> ActivateOrganisationAsync(int id, string updatedBy);
        //Task<Organisation> GetOrganisationById(int id);
        //Task<bool> Save();
        //Task<List<string>> GetLeadContact();
        //Task<List<Address>> GetAddress();
        Task<bool> CreateOrganisationAsync(int contactId, CreateOrganisationDto createOrganisationDto, string createdBy);
        Task<List<GetOrganisationPremisesModel>> GetOrganisationPremisesAsync(int id);
        //Task<Organisation> GetOrganisationByDirectorate(int directorateId);
        public Task<bool> ChangeOgranisationIsActiveAsync(int oganisationId, bool isActive);
    }
}
