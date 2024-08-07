using Common.Helper;
using Common.Models;
using CommonWeb.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IDepartmentService
    {
        Task<PagedList<DepartmentDto>> GetDepartmentAsync(DepartmentParams departmentParams);
        Task<bool> ActivateDepartmentAsync(int id, string updatedBy);
        Task<bool> AddDepartmentAsync(int directorateId, int contactId, CreateDepartmentDto createDepartment, string createdBy);
        Task<bool> ChangeDepartmentIsActiveAsync(int departmentId, bool isActive);

        Task<int> CheckDepartmentName(string name);
        Task<DepartmentDto?> UpdateDepartment(UpdateDepartmentModel updateDepartmentModel, string updatedBy);
        Task<bool> CheckAddress(int departmentId);
        Task<DepartmentDto?> CreateDepartment(CreateDepartmentModel createDepartmentModel, string createdBy);
    }
}
