using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Extensions;
using BusinessLayer.Interfaces;
using CommonWeb.Dto;
using Common.Helper;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class DirectorateService : IDirectorateService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAddressService _addressService;
        private readonly ICompanyContactService _companyContactService;
        public DirectorateService(DataContext context, IMapper mapper, ICompanyContactService companyContactService, IAddressService addressService)
        {
            _context= context;
            _mapper= mapper;
            _addressService = addressService;
            _companyContactService = companyContactService;
        }
        public async Task<PagedList<DirectorateDto>> GetDirectorateAsync(DirectorateParams directorateParams)
        {
            var query = _context.Directorates
                .Search(directorateParams.SearchTerm)
                .AsQueryable();
            if (directorateParams.FirstTenPart)
            {
                query = query.TakeFirstTen();
            }

            if (!directorateParams.InActive)
            {
                query = query.Active(directorateParams.InActive);
            }

            var directoratesQuery = query.ProjectTo<DirectorateDto>(_mapper.ConfigurationProvider);

            return await PagedList<DirectorateDto>.ToPagedList(directoratesQuery, directorateParams.PageNumber, directorateParams.PageSize);
        }
        public async Task<bool> ActivateDirectorateAsync(int id, string updatedBy)
        {
            var directorate = await _context.Directorates.FindAsync(id);

            if (directorate == null)
            {
                return false;
            }

            if (directorate.IsActive)
            {
                throw new InvalidOperationException("Directorate is already active.");
            }

            directorate.IsActive = true;
            directorate.UpdatedDate = DateTime.Now;
            directorate.UpdatedBy = updatedBy;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddDirectorateAsync(int organisationId, int contactId, CreateDirectorateDto createDirectorate, string createdBy)
        {
            if (createDirectorate == null)
            {
                throw new ArgumentNullException(nameof(createDirectorate), "Invalid organisation data");
            }

            // Check existence of Directorate
            var organisationExists = await _context.Organisations.AnyAsync(c => c.OrganisationId == organisationId);
            if (!organisationExists)
            {
                throw new ArgumentException("Invalid organisationId");
            }

            var contactExists = await _context.Contacts.AnyAsync(c => c.ContactId == contactId);
            if (!contactExists)
            {
                throw new ArgumentException("Invalid contactId");
            }

            Address addressEntity = null;
            if (createDirectorate.GetAddressFrom == "Organisation")
            {
                addressEntity = await _addressService.GetAddressByOrganisationId(organisationId);
            }
            //if (!createDirectorate.Address.PostCode.IsNullOrEmpty())
            else 
            {
                addressEntity = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.PostCode == createDirectorate.Address.PostCode);
            }

            if (addressEntity == null)
            {
                // Tạo mới địa chỉ nếu không tồn tại
                addressEntity = await _addressService.HandleAddressAsync(createDirectorate.Address, createdBy);
            }
            CompanyContact companyContact = null;
            var companyContactByOrganisation = await _companyContactService.GetCompanyContactByOrganisationId(organisationId);
            if (createDirectorate.CompanyContact.TypeOfBusiness.IsNullOrEmpty() ||
                createDirectorate.CompanyContact.SICCode.IsNullOrEmpty() ||
                createDirectorate.CompanyContact.WebAddress.IsNullOrEmpty())
            {

                createDirectorate.CompanyContact.TypeOfBusiness = companyContactByOrganisation.TypeOfBusiness;
                createDirectorate.CompanyContact.SICCode = companyContactByOrganisation.SICCode;
                createDirectorate.CompanyContact.WebAddress = companyContactByOrganisation.WebAddress;
                if(
                    createDirectorate.CompanyContact.FullDescription!= companyContactByOrganisation.FullDescription
                    ||createDirectorate.CompanyContact.PhoneNumber != companyContactByOrganisation.PhoneNumber
                    || createDirectorate.CompanyContact.Fax != companyContactByOrganisation.Fax
                    || createDirectorate.CompanyContact.Email != companyContactByOrganisation.Email
                    || createDirectorate.CompanyContact.CharityNumber != companyContactByOrganisation.CharityNumber
                    || createDirectorate.CompanyContact.CompanyNumber != companyContactByOrganisation.CompanyNumber
                    )
                {
                    companyContact = new CompanyContact
                    {
                        TypeOfBusiness = createDirectorate.CompanyContact.TypeOfBusiness,
                        SICCode = createDirectorate.CompanyContact.SICCode,
                        WebAddress = createDirectorate.CompanyContact.WebAddress,
                        FullDescription = createDirectorate.CompanyContact.FullDescription,
                        PhoneNumber = createDirectorate.CompanyContact.PhoneNumber,
                        Fax = createDirectorate.CompanyContact.Fax,
                        Email = createDirectorate.CompanyContact.Email,
                        CharityNumber = createDirectorate.CompanyContact.CharityNumber,
                        CompanyNumber = createDirectorate.CompanyContact.CompanyNumber,
                        CreatedBy = createdBy,
                        CreatedDate = DateTime.Now
                    };
                }    
                else companyContact = await _context.CompanyContacts.Where(cc => cc.SICCode == companyContactByOrganisation.SICCode).FirstOrDefaultAsync();
                //companyContact = _mapper.Map<CompanyContact>(createDirectorate.CompanyContact);
            }
            else
            {
                var companyContactEntity = await _companyContactService.HandleCompanyContactAsync(createDirectorate.CompanyContact, createdBy);
                companyContact = companyContactEntity;
            }
            //var companyContactEntity = await _companyContactService.HandleCompanyContactAsync(createDirectorate.CompanyContact, createdBy);

            var directorateMap = _mapper.Map<Directorate>(createDirectorate);
            directorateMap.OrganisationId = organisationId;
            directorateMap.ContactId = contactId;
            directorateMap.Address = addressEntity;
            directorateMap.CompanyContact = companyContact;
            directorateMap.CreatedBy = createdBy;
            directorateMap.CreatedDate = DateTime.Now;
            directorateMap.IsActive = false;

            _context.Directorates.Add(directorateMap);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateDirectorateAsync(int organisationId, int contactId, UpdateDirectorateDto updateDirectorate, string updatedBy)
        {
            if (updateDirectorate == null)
            {
                throw new ArgumentNullException(nameof(updateDirectorate), "Invalid directorate data");
            }

            // Check existence of Organisation
            var organisationExists = await _context.Organisations.AnyAsync(c => c.OrganisationId == organisationId);
            if (!organisationExists)
            {
                throw new ArgumentException("Invalid organisationId");
            }

            // Check existence of Contact
            var contactExists = await _context.Contacts.AnyAsync(c => c.ContactId == contactId);
            if (!contactExists)
            {
                throw new ArgumentException("Invalid contactId");
            }

            // Retrieve the existing Directorate
            var directorate = await _context.Directorates
                .Include(d => d.Address)
                .Include(d => d.CompanyContact)
                .FirstOrDefaultAsync(d => d.DirectorateId == updateDirectorate.DirectorateId);

            if (directorate == null)
            {
                throw new ArgumentException("Directorate not found");
            }

            // Handle Address
            Address addressEntity = null;
            if (updateDirectorate.GetAddressFrom == "Organisation")
            {
                addressEntity = await _addressService.GetAddressByOrganisationId(organisationId);
            }
            else
            {
                addressEntity = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.PostCode == updateDirectorate.Address.PostCode);
            }

            if (addressEntity == null)
            {
                addressEntity=await _addressService.HandleAddressAsync(updateDirectorate.Address, updatedBy);
            }
            else
            {
                //var address=_context.Addresses.FirstOrDefaultAsync(a => a.PostCode == createDirectorate.Address.PostCode);
                // Update existing address
                //_mapper.Map(createDirectorate.Address, addressEntity);
                addressEntity.UpdatedBy = updatedBy;
                addressEntity.UpdatedDate = DateTime.Now;
                _context.Addresses.Update(addressEntity);
                await _context.SaveChangesAsync(); // Save the address before proceeding
            }

            // Handle CompanyContact
            //CompanyContact companyContact = null;
            //var companyContactByOrganisation = await _companyContactService.GetCompanyContactByOrganisationId(organisationId);
            //var existingCompanyContact = await _context.CompanyContacts
            //    .FirstOrDefaultAsync(cc => cc.PhoneNumber == updateDirectorate.CompanyContact.PhoneNumber);
            //if (existingCompanyContact != null)
            //{
            //    companyContact = existingCompanyContact;
            //    companyContact.UpdatedBy = updatedBy;
            //    companyContact.UpdatedDate = DateTime.Now;
            //    await _context.SaveChangesAsync();
            //}

            //else
            //{
            //    var newcompanyContact = _mapper.Map<CompanyContact>(updateDirectorate.CompanyContact);
            //    newcompanyContact.CreatedBy = updatedBy;
            //    newcompanyContact.CreatedDate=DateTime.Now;
            //    _context.CompanyContacts.Add(newcompanyContact);
            //    await _context.SaveChangesAsync(); // Save the company contact before proceeding
            //}

            CompanyContact companyContact;
            if (updateDirectorate.CompanyContact == null ||
                string.IsNullOrEmpty(updateDirectorate.CompanyContact.TypeOfBusiness) ||
                string.IsNullOrEmpty(updateDirectorate.CompanyContact.SICCode) ||
                string.IsNullOrEmpty(updateDirectorate.CompanyContact.WebAddress))
            {
                companyContact = await _companyContactService.GetCompanyContactByOrganisationId(organisationId);
            }
            else
            {
                companyContact = await _context.CompanyContacts
                    .FirstOrDefaultAsync(cc => cc.PhoneNumber == updateDirectorate.CompanyContact.PhoneNumber);

                if (companyContact == null)
                {
                    companyContact = await _companyContactService.HandleCompanyContactAsync(updateDirectorate.CompanyContact, updatedBy);
                    //companyContact = _mapper.Map<CompanyContact>(updateDirectorate.CompanyContact);
                    //companyContact.CreatedBy = updatedBy;
                    //companyContact.CreatedDate = DateTime.Now;
                    //_context.CompanyContacts.Add(companyContact);
                }
                else
                {
                    companyContact.UpdatedBy = updatedBy;
                    companyContact.UpdatedDate = DateTime.Now;
                    _context.CompanyContacts.Update(companyContact);
                }

                await _context.SaveChangesAsync(); // Save the company contact before proceeding
            }

            // Update Directorate properties
            //_mapper.Map(createDirectorate, directorate);
            directorate.Name = updateDirectorate.Name;
            directorate.ShortDescription = updateDirectorate.ShortDescription;
            directorate.LeadContact = updateDirectorate.LeadContact;
            directorate.Address = addressEntity;
            directorate.CompanyContact = companyContact;
            directorate.UpdatedBy = updatedBy;
            directorate.UpdatedDate = DateTime.Now;
            directorate.IsActive=updateDirectorate.IsActive;

            _context.Directorates.Update(directorate);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangeDirectorateIsActiveAsync(int directorateId, bool isActive)
        {
            var directorate = await _context.Directorates.FindAsync(directorateId);
            if (directorate == null)
            {
                return false;
            }

            directorate.IsActive = !directorate.IsActive;
            await _context.SaveChangesAsync();

            return true;
        }

    }
}




//{
//    "name": "Liver",
//  "shortDescription": "string",
//  "leadContact": "string",
//  "address": {
//        "address1": "",
//    "address2": "",
//    "address3": "",
//    "postCode": "",
//    "city": "",
//    "townId": 0
//  },
//  "companyContact": {
//        "phoneNumber": "456",
//    "fax": "555",
//    "email": "Liver@gmail.com",
//    "webAddress": "",
//    "charityNumber": "666",
//    "companyNumber": "999",
//    "typeOfBusiness": "",
//    "sicCode": "",
//    "fullDescription": "In England"
//  },
//  "getAddressFrom": "Organisation"
//}