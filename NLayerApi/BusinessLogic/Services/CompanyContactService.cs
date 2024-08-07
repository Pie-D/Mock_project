using AutoMapper;
using BusinessLayer.Interfaces;
using Common.Models;
using CommonWeb.Dto;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CompanyContactService : ICompanyContactService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public CompanyContactService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CompanyContact> GetCompanyContactOrganisationByDepartment(int departmentId)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (department == null)
            {
                return null;
            }

            var directorate = await _context.Directorates.FirstOrDefaultAsync(d => d.DirectorateId == department.DirectorateId);

            if (directorate == null)
            {
                return null;
            }

            var organisation = await _context.Organisations.FirstOrDefaultAsync(d => d.OrganisationId == directorate.OrganisationId);

            if (organisation == null)
            {
                return null;
            }
            var companyContact=await _context.CompanyContacts.FirstOrDefaultAsync(cc=>cc.CompanyContactId== organisation.CompanyContactId);
            return companyContact;
        }

        public async Task<CompanyContact> GetCompanyContactByOrganisationId(int organisationId)
        {
            var organisation = await _context.Organisations
                .FirstOrDefaultAsync(o => o.OrganisationId == organisationId);

            if (organisation == null)
            {
                return null; // Or handle the case when organisation is not found
            }

            var companyContact = await _context.CompanyContacts.FirstOrDefaultAsync(a => a.CompanyContactId == organisation.CompanyContactId);
            return companyContact;
        }

        public async Task<string> GetSICCode(string typeOfBusiness)
        {
            var sicCode = await _context.CompanyContacts
                                        .Where(s => s.TypeOfBusiness == typeOfBusiness)
                                        .Select(s => s.SICCode)
                                        .FirstOrDefaultAsync();

            return sicCode;
        }

        public async Task<List<string>> GetTypeOfBusiness()
        {
            var typeOfBusinesses=await _context.CompanyContacts.Select(cc=>cc.TypeOfBusiness).Distinct().ToListAsync();
            return typeOfBusinesses;
        }

        public async Task<CompanyContact> HandleCompanyContactAsync(CompanyContactDto companyContactDto, string createdBy)
        {
            //if (companyContactDto == null)
            //    return null;

            //var companyContactEntity = await _context.CompanyContacts
            //    .FirstOrDefaultAsync(cc => cc.PhoneNumber == companyContactDto.PhoneNumber);

            //if (companyContactEntity == null)
            //{
                 var companyContact = _mapper.Map<CompanyContact>(companyContactDto);
                if (string.IsNullOrEmpty(companyContact.PhoneNumber))
                {
                    throw new ArgumentException("Company contact information is missing or invalid.");
                }

                companyContact.CreatedBy = createdBy;
                companyContact.CreatedDate = DateTime.Now;
                _context.CompanyContacts.Add(companyContact);
                await _context.SaveChangesAsync();
            //}

            return companyContact;
        }

        public async Task<CompanyContact> GetCompanyContactOrganisationByDirectorate(int directorateId)
        {

            var directorate = await _context.Directorates.FirstOrDefaultAsync(d => d.DirectorateId == directorateId);

            if (directorate == null)
            {
                return null;
            }

            var organisation = await _context.Organisations.FirstOrDefaultAsync(d => d.OrganisationId == directorate.OrganisationId);

            if (organisation == null)
            {
                return null;
            }
            var companyContact = await _context.CompanyContacts.FirstOrDefaultAsync(cc => cc.CompanyContactId == organisation.CompanyContactId);
            return companyContact;
        }

        public async Task<bool> UpdateCompanyContact(UpdateCompanyContactModel updateCompanyContactModel, string updatedBy)
        {
            var companyContactFind = await _context.CompanyContacts.FirstOrDefaultAsync(p =>
                p.CompanyContactId == updateCompanyContactModel.CompanyContactIdUpdate);
            if (companyContactFind == null) return false;
            var companyContactDto = updateCompanyContactModel.CompanyContactDto;
            companyContactFind.UpdatedBy = updatedBy;
            companyContactFind.UpdatedDate = DateTime.Now;
            companyContactFind.Email = companyContactDto.Email;
            companyContactFind.Fax = companyContactDto.Fax;
            companyContactFind.FullDescription = companyContactDto.FullDescription;
            companyContactFind.PhoneNumber = companyContactDto.PhoneNumber;
            _context.CompanyContacts.Update(companyContactFind);
            var result = await _context.SaveChangesAsync() > 0;
            return result;

        }

        public async Task<CompanyContactDto?> CreateCompanyContact(CreateCompanyContactModel createCompanyContactModel, string createdBy)
        {
            var companyContact = _mapper.Map<CompanyContact>(createCompanyContactModel);
            companyContact.CreatedBy = createdBy;
            companyContact.CreatedDate = DateTime.Now;
            await _context.CompanyContacts.AddAsync(companyContact);
            var result = await _context.SaveChangesAsync() > 0;
            var companyContactDto = _mapper.Map<CompanyContactDto>(companyContact);
            return result ? companyContactDto : null;
        }
    }
}
