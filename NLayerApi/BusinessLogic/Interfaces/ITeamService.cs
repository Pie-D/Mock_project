using CommonWeb.Dto;
using Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;

namespace BusinessLayer.Interfaces
{
    public interface ITeamService
    {
        Task<PagedList<TeamDto>> GetTeamAsync(TeamParams teamParams);
        Task<bool> ActivateTeamAsync(int id, string updatedBy);
        Task<bool> AddTeamAsync(int departmentId, int contactId, CreateTeamDto createTeam, string createdBy);
        Task<bool> ChangeTeamIsActiveAsync(int teamId, bool isActive);
        Task<bool> UpdateTeamAsync(int departmentId, int contactId, UpdateTeamDto updateTeam, string updatedBy);
    }
}
