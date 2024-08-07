using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Extensions;
using BusinessLayer.Interfaces;
using CommonWeb.Dto;
using Common.Helper;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace BusinessLayer.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IAddressService _addressService;
        private readonly ICompanyContactService _companyContactService;
        public DepartmentService(DataContext context, IMapper mapper, ICompanyContactService companyContactService, IAddressService addressService)
        {
            _context = context;
            _mapper = mapper;
            _addressService = addressService;
            _companyContactService = companyContactService;
        }
        public async Task<PagedList<DepartmentDto>> GetDepartmentAsync(DepartmentParams departmentParams)
        {
            var query = _context.Departments
                .Search(departmentParams.SearchTerm)
                .AsQueryable();
            if (departmentParams.FirstTenPart)
            {
                query = query.TakeFirstTen();
            }

            if (!departmentParams.InActive)
            {
                query = query.Active(departmentParams.InActive);
            }

            var departmentsQuery = query.ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider);

            return await PagedList<DepartmentDto>.ToPagedList(departmentsQuery, departmentParams.PageNumber, departmentParams.PageSize);
        }
        public async Task<bool> ActivateDepartmentAsync(int id, string updatedBy)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return false;
            }

            if (department.IsActive)
            {
                throw new InvalidOperationException("Department is already active.");
            }

            department.IsActive = true;
            department.UpdatedDate = DateTime.Now;
            department.UpdatedBy = updatedBy;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddDepartmentAsync(int directorateId, int contactId, CreateDepartmentDto createDepartment, string createdBy)
        {
            if (createDepartment == null)
            {
                throw new ArgumentNullException(nameof(createDepartment), "Invalid organisation data");
            }

            // Check existence of Directorate
            var directorateExists = await _context.Directorates.AnyAsync(c => c.DirectorateId == directorateId);
            if (!directorateExists)
            {
                throw new ArgumentException("Invalid directorateId");
            }

            var contactExists = await _context.Contacts.AnyAsync(c => c.ContactId == contactId);
            if (!contactExists)
            {
                throw new ArgumentException("Invalid contactId");
            }

            Address addressEntity = null;
            if (createDepartment.GetAddressFrom == "Directorate")
            {
                addressEntity = await _addressService.GetAddressByDirectorate(directorateId);
            }
            else if (createDepartment.GetAddressFrom == "Organisation")
            {
                addressEntity = await _addressService.GetAddressByOrganisation(directorateId);
            }
            else
            {
                addressEntity = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.PostCode == createDepartment.Address.PostCode);
            }

            //if (addressEntity == null)
            //{
            //    addressEntity = _mapper.Map<Address>(addressDto);
            //    if (string.IsNullOrEmpty(addressEntity.PostCode))
            //    {
            //        throw new ArgumentException("Address information is missing or invalid.");
            //    }

            //    addressEntity.CreatedBy = createdBy;
            //    addressEntity.CreatedDate = DateTime.Now;
            //    _context.Addresses.Add(addressEntity);
            //    await _context.SaveChangesAsync();
            //}
            if (addressEntity == null)
            {
                // Tạo mới địa chỉ nếu không tồn tại
                _addressService.HandleAddressAsync(createDepartment.Address, createdBy);
            }

            //CompanyContact companyContactEntity = null;

            //companyContactEntity = await _context.CompanyContacts
            //    .FirstOrDefaultAsync(c => c.PhoneNumber == createDepartment.CompanyContact.PhoneNumber);
            //if (companyContactEntity == null)
            //{
            //    companyContactEntity = await _companyContactService.HandleCompanyContactAsync(createDepartment.CompanyContact, createdBy);
            //}
            CompanyContact companyContact;
            if (createDepartment.CompanyContact == null ||
                string.IsNullOrEmpty(createDepartment.CompanyContact.TypeOfBusiness) ||
                string.IsNullOrEmpty(createDepartment.CompanyContact.SICCode) ||
                string.IsNullOrEmpty(createDepartment.CompanyContact.WebAddress))
            {

                companyContact = await _companyContactService.GetCompanyContactOrganisationByDirectorate(directorateId);
            }
            else
            {
                companyContact = await _context.CompanyContacts
                    .FirstOrDefaultAsync(cc => cc.PhoneNumber == createDepartment.CompanyContact.PhoneNumber);

                if (companyContact == null)
                {
                    companyContact = await _companyContactService.HandleCompanyContactAsync(createDepartment.CompanyContact, createdBy);
                    //companyContact = _mapper.Map<CompanyContact>(updateDirectorate.CompanyContact);
                    //companyContact.CreatedBy = updatedBy;
                    //companyContact.CreatedDate = DateTime.Now;
                    //_context.CompanyContacts.Add(companyContact);
                }
                else
                {
                    companyContact.UpdatedBy = createdBy;
                    companyContact.UpdatedDate = DateTime.Now;
                    _context.CompanyContacts.Update(companyContact);
                }

                await _context.SaveChangesAsync(); // Save the company contact before proceeding
            }

            var departmentMap = _mapper.Map<Department>(createDepartment);
            departmentMap.DirectorateId = directorateId;
            departmentMap.ContactId = contactId;
            departmentMap.Address = addressEntity;
            departmentMap.CompanyContact = companyContact;
            departmentMap.CreatedBy = createdBy;
            departmentMap.CreatedDate = DateTime.Now;
            departmentMap.IsActive = false;

            _context.Departments.Add(departmentMap);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangeDepartmentIsActiveAsync(int departmentId, bool isActive)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
            {
                return false;
            }

            department.IsActive = !department.IsActive;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> CheckDepartmentName(string name)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(p => p.Name == name);
            return department?.DepartmentId ?? 0;
        }

        public async Task<DepartmentDto?> UpdateDepartment(UpdateDepartmentModel updateDepartmentModel, string updatedBy)
        {
            var departmentDto = updateDepartmentModel.DepartmentDto;
            var addressDto = updateDepartmentModel.AddressDto;
            var departmentId = updateDepartmentModel.DepartmentId;
            var copyAddressFrom = updateDepartmentModel.CopyAddressFrom;
            var addressIdUpdate = departmentDto?.AddressId;
            var departmentFind = await _context.Departments.FirstOrDefaultAsync(s => s.DepartmentId == departmentId);
            if (departmentFind == null)
            {
                throw new ArgumentException("Invalid DepartmentId");
            }
            var checkName = await CheckDepartmentName(departmentDto.Name);
            if (checkName != 0 && checkName != departmentId)
            {
                throw new ArgumentException("Department Name already Exist");
            }

            if (copyAddressFrom == "none" && !await CheckAddress(departmentId))
            {
                bool check = await _addressService.CheckPostCode(addressDto);
                if (check)
                {
                    var address = await _addressService.AddAddress(addressDto, updatedBy);
                    if (address != null) addressIdUpdate = address.AddressId;
                }
                else
                {
                    throw new ArgumentException("Postcode Invalid");
                }
            }

            if (copyAddressFrom == "none" && await CheckAddress(departmentId))
            {

                bool check = await _addressService.CheckPostCode(addressDto);
                if (check)
                {
                    var updateAddressModel = new UpdateAddressModel
                    {
                        AddressIdUpdate = departmentDto.AddressId,
                        AddressDto = addressDto
                    };
                    var address = await _addressService.UpdateAddress(updateAddressModel, updatedBy);
                    if (address == null)
                    {
                        throw new ArgumentException("Update Address Fail");
                    }
                }
                else
                {
                    throw new ArgumentException("Postcode Invalid");
                }
            }

            if (copyAddressFrom != "none")
            {
                var directorate =
                    await _context.Directorates.FirstOrDefaultAsync(p => p.DirectorateId == departmentDto.DirectorateId);
                var organisation =
                    await _context.Organisations.FirstOrDefaultAsync(p => p.OrganisationId == directorate.OrganisationId);
                if (copyAddressFrom == "Organisation")
                {
                    addressIdUpdate = organisation.AddressId;
                }

                if (copyAddressFrom == "Directorate")
                {
                    addressIdUpdate = directorate.AddressId;
                }
            }

            var companyContact =
                await _context.CompanyContacts.FirstOrDefaultAsync(p =>
                    p.CompanyContactId == departmentDto.CompanyContactId);

            var companyContactDto = _mapper.Map<CompanyContactDto>(companyContact);
            companyContactDto.PhoneNumber = updateDepartmentModel.PhoneNumber;
            companyContactDto.Email = updateDepartmentModel.Email;
            companyContactDto.Fax = updateDepartmentModel.Fax;
            companyContactDto.FullDescription = updateDepartmentModel.DepartmentFullDescription;

            var companyContactUpdate = new UpdateCompanyContactModel
            {
                CompanyContactDto = companyContactDto,
                CompanyContactIdUpdate = departmentDto.CompanyContactId
            };
            var checkUpdateCompanyContact = await _companyContactService.UpdateCompanyContact(companyContactUpdate, updatedBy);
            if (!checkUpdateCompanyContact)
            {
                throw new ArgumentException("Update CompanyContact Fail");
            }
            departmentFind.CompanyContactId = departmentDto.CompanyContactId;
            departmentFind.UpdatedBy = updatedBy;
            departmentFind.UpdatedDate = DateTime.Now;
            departmentFind.AddressId = addressIdUpdate;
            departmentFind.LeadContact = departmentDto.LeadContact;
            departmentFind.ShortDescription = departmentDto.ShortDescription;
            departmentFind.Name = departmentDto.Name;
            _context.Departments.Update(departmentFind);
            var result = await _context.SaveChangesAsync() > 0;
            var departmentResult = _mapper.Map<DepartmentDto>(departmentFind);
            return result ? departmentResult : null;
        }

        public async Task<bool> CheckAddress(int departmentId)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(p => p.DepartmentId == departmentId);

            int? addressId = department.AddressId;
            var directorateId = department.DirectorateId;
            var directorate = await _context.Directorates.FirstOrDefaultAsync(p => p.DirectorateId == directorateId);
            var organisationId = directorate.OrganisationId;
            var organisation = await _context.Organisations.FirstOrDefaultAsync(p => p.OrganisationId == organisationId);
            if (addressId != directorate.AddressId && addressId != organisation.AddressId) return true;

            return false;
        }
        public async Task<DepartmentDto?> CreateDepartment(CreateDepartmentModel createDepartmentModel, string createdBy)
        {
            int addressCopyId = 1;
            var departmentDto = createDepartmentModel.DepartmentDto;
            var addressDto = createDepartmentModel.AddressDto;
            var copyAddressFrom = createDepartmentModel.CopyAddressFrom;

            int checkName = await CheckDepartmentName(departmentDto?.Name);
            if (checkName != 0)
            {
                throw new ArgumentException("Department Name already Exist");
            }
            if (copyAddressFrom == "none")
            {
                var check = await _addressService.CheckPostCode(addressDto);
                if (check)
                {
                    var address = await _addressService.AddAddress(addressDto, createdBy);
                    if (address != null) addressCopyId = address.AddressId;
                }
                else
                {
                    throw new ArgumentException("Postcode Invalid");
                }

            }
            var directorate =
                await _context.Directorates.FirstOrDefaultAsync(p => p.DirectorateId == departmentDto.DirectorateId);
            var organisation =
                await _context.Organisations.FirstOrDefaultAsync(p => p.OrganisationId == directorate.OrganisationId);
            if (copyAddressFrom == "Organisation")
            {
                addressCopyId = organisation.AddressId;
            }
            if (copyAddressFrom == "Directorate")
            {
                addressCopyId = directorate.AddressId;
            }
            var companyContact =
                await _context.CompanyContacts.FirstOrDefaultAsync(p =>
                    p.CompanyContactId == organisation.CompanyContactId);

            var companyContactCreate = _mapper.Map<CreateCompanyContactModel>(companyContact);
            companyContactCreate.PhoneNumber = createDepartmentModel.PhoneNumber;
            companyContactCreate.Email = createDepartmentModel.Email;
            companyContactCreate.Fax = createDepartmentModel.Fax;
            companyContactCreate.FullDescription = createDepartmentModel.DepartmentFullDescription;
            var companyContactCreated = await _companyContactService.CreateCompanyContact(companyContactCreate, createdBy);

            var departmentMap = _mapper.Map<Department>(departmentDto);
            departmentMap.CreatedBy = createdBy;
            departmentMap.CreatedDate = DateTime.UtcNow;
            departmentMap.AddressId = addressCopyId;
            departmentMap.CompanyContactId = companyContactCreated.CompanyContactId;
            await _context.Departments.AddAsync(departmentMap);
            var result = await _context.SaveChangesAsync() > 0;
            var departmentResult = _mapper.Map<DepartmentDto>(departmentMap);
            return result ? departmentResult : null;
        }
    }
}
