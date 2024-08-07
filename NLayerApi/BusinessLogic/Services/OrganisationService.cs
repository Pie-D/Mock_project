using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Extensions;
using BusinessLayer.Interfaces;
using CommonWeb.Dto;
using Common.Helper;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommonWeb.Models;

namespace BusinessLayer.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IAddressService _addressService;
        private readonly ICompanyContactService _companyContactService;
        public OrganisationService(DataContext context, IMapper mapper, ICompanyContactService companyContactService, IAddressService addressService)
        {
            _context = context;
            _mapper = mapper;
            _addressService = addressService;
            _companyContactService = companyContactService;
        }

        public async Task<PagedList<OrganisationDto>> GetOrganisationAsync(OrganisationParams organisationParams)
        {
            var query = _context.Organisations
                .Search(organisationParams.SearchTerm)
                .AsQueryable();
            if (organisationParams.FirstTenPart)
            {
                query = query.TakeFirstTen();
            }

            if (!organisationParams.InActive)
            {
                query = query.Active(organisationParams.InActive);
            }

            var organisationsQuery = query.ProjectTo<OrganisationDto>(_mapper.ConfigurationProvider);

            return await PagedList<OrganisationDto>.ToPagedList(organisationsQuery, organisationParams.PageNumber, organisationParams.PageSize);
        }
        public async Task<bool> ActivateOrganisationAsync(int id, string updatedBy)
        {
            var organisation = await _context.Organisations.FindAsync(id);

            if (organisation == null)
            {
                return false;
            }

            if (organisation.IsActive)
            {
                throw new InvalidOperationException("Organisation is already active.");
            }

            organisation.IsActive = true;
            organisation.UpdatedDate = DateTime.Now;
            organisation.UpdatedBy = updatedBy;
            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<List<string>> GetLeadContact()
        //{
        //    var leadContacts = await _context.Organisations.Select(o => o.LeadContact).Distinct().ToListAsync();
        //    return leadContacts;
        //}

        //public async Task<List<Address>> GetAddress()
        //{
        //    var addresses = await _context.Organisations.Select(o => o.Address).ToListAsync();
        //    return addresses;
        //}

        public async Task<bool> CreateOrganisationAsync(int contactId, CreateOrganisationDto createOrganisationDto, string createdBy)
        {
            if (createOrganisationDto == null)
                throw new ArgumentNullException(nameof(createOrganisationDto), "Invalid organisation data");

            // Kiểm tra xem tổ chức đã tồn tại chưa
            var existingOrganisation = await _context.Organisations
                .Where(o => o.OrgName.Trim().ToUpper() == createOrganisationDto.OrgName.Trim().ToUpper())
                .FirstOrDefaultAsync();

            if (existingOrganisation != null)
            {
                throw new InvalidOperationException("Organisation already exists");
            }

            // Kiểm tra sự tồn tại của Contact
            var contactExists = await _context.Contacts.AnyAsync(c => c.ContactId == contactId);
            if (!contactExists)
            {
                throw new ArgumentException("Invalid ContactId");
            }
            Address addressEntity = null;
            addressEntity = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.PostCode == createOrganisationDto.Address.PostCode);
              
            if(addressEntity == null)
            {
                addressEntity = await _addressService.HandleAddressAsync(createOrganisationDto.Address, createdBy);
            }


            CompanyContact companyContactEntity = null;

            companyContactEntity = await _context.CompanyContacts
                .FirstOrDefaultAsync(c => c.PhoneNumber == createOrganisationDto.CompanyContact.PhoneNumber);

            if (companyContactEntity == null)
            {
                companyContactEntity = await _companyContactService.HandleCompanyContactAsync(createOrganisationDto.CompanyContact, createdBy);
            }
            var organisationMap = _mapper.Map<Organisation>(createOrganisationDto);
            organisationMap.Address = addressEntity;
            organisationMap.CompanyContact = companyContactEntity;
            organisationMap.ContactId = contactId;
            organisationMap.CreatedBy = createdBy;
            organisationMap.CreatedDate = DateTime.UtcNow;
            organisationMap.IsActive = false;

            if (createOrganisationDto.ReferenceDataIds.Any())
            {
                organisationMap.OrganisationReferenceDatas = createOrganisationDto.ReferenceDataIds
                    .Select(refId => new OrganisationReferenceData
                    {
                        RefId = refId,
                        Organisation = organisationMap
                    }).ToList();
            }
            _context.Organisations.Add(organisationMap);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<GetOrganisationPremisesModel>> GetOrganisationPremisesAsync(int id )
        {
            List<int> servicesIds = await _context.OrganisationServices
                .Where(s=>s.OrganisationId == id)
                .Select(s=>s.ServiceId)
                .ToListAsync();


            var premises = await _context.Premises
                .Where(s => servicesIds.Contains(s.ServiceId))
                .ToListAsync();

            return _mapper.Map<List<GetOrganisationPremisesModel>>(premises);
        }

        //public async Task<Organisation> GetOrganisationByDirectorate(int directorateId)
        //{
        //    var directorate = await _context.Directorates
        //        .FirstOrDefaultAsync(x => x.DirectorateId == directorateId);

        //    if (directorate == null)
        //        return null;

        //    var organisation = await _context.Organisations
        //        .FirstOrDefaultAsync(o => o.OrganisationId == directorate.OrganisationId);

        //    return organisation;
        //}

        public async Task<bool> ChangeOgranisationIsActiveAsync(int organisationId, bool isActive)
        {
            var organisation = await _context.Organisations.FindAsync(organisationId);
            if (organisation == null)
            {
                return false;
            }

            organisation.IsActive = !organisation.IsActive;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}