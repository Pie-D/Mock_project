using CommonWeb.Dto;
using Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IDirectorateService
    {
        Task<PagedList<DirectorateDto>> GetDirectorateAsync(DirectorateParams directorateParams);
        Task<bool> ActivateDirectorateAsync(int id, string updatedBy);
        Task<bool> AddDirectorateAsync(int organisationId, int contactId, CreateDirectorateDto createDirectorate, string createdBy);

        Task<bool> UpdateDirectorateAsync(int organisationId, int contactId, UpdateDirectorateDto updateDirectorate, string updatedBy);
        Task<bool> ChangeDirectorateIsActiveAsync(int directorateId, bool isActive);
    }
}
