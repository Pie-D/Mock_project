using Common.Models;
using CommonWeb.Dto;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICompanyContactService
    {
        Task<List<string>> GetTypeOfBusiness();
        Task<string> GetSICCode(string typeOfBusiness);
        Task<CompanyContact> HandleCompanyContactAsync(CompanyContactDto companyContactDto, string createdBy);
        Task<CompanyContact> GetCompanyContactByOrganisationId(int organisationId);
        Task<CompanyContact> GetCompanyContactOrganisationByDepartment(int departmentId);
        Task<CompanyContact> GetCompanyContactOrganisationByDirectorate(int directorateId);

        Task<bool> UpdateCompanyContact(UpdateCompanyContactModel updateCompanyContactModel, string updateddBy);
        Task<CompanyContactDto?> CreateCompanyContact(CreateCompanyContactModel createCompanyContactModel, string createdBy);


        //Task<CompanyContactDto> GetCompanyContactById(int organisationId);
        //Task<CompanyContactDto?> CreateCompanyContact(CreateCompanyContactModel createCompanyContactModel, string createdBy);
        //Task<bool> UpdateCompanyContact(UpdateCompanyContactModel updateCompanyContactModel, string updateddBy);

    }
}
