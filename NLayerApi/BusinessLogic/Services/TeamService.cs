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
using Common.Dto;

namespace BusinessLayer.Services
{
    public class TeamService : ITeamService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper; 
        private readonly IAddressService _addressService;
        private readonly ICompanyContactService _companyContactService;
        public TeamService(DataContext context, IMapper mapper, ICompanyContactService companyContactService, IAddressService addressService)
        {
            _context = context;
            _mapper = mapper;
            _addressService = addressService;
            _companyContactService = companyContactService;
        }
        public async Task<PagedList<TeamDto>> GetTeamAsync(TeamParams teamParams)
        {
            var query = _context.Teams
                .Search(teamParams.SearchTerm)
                .AsQueryable();
            if (teamParams.FirstTenPart)
            {
                query = query.TakeFirstTen();
            }

            if (!teamParams.InActive)
            {
                query = query.Active(teamParams.InActive);
            }

            var teamsQuery = query.ProjectTo<TeamDto>(_mapper.ConfigurationProvider);

            return await PagedList<TeamDto>.ToPagedList(teamsQuery, teamParams.PageNumber, teamParams.PageSize);
        }
        public async Task<bool> ActivateTeamAsync(int id, string updatedBy)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return false;
            }

            if (team.IsActive)
            {
                throw new InvalidOperationException("Team is already active.");
            }

            team.IsActive = true;
            team.UpdatedDate = DateTime.Now;
            team.UpdatedBy = updatedBy;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddTeamAsync(int departmentId, int contactId, CreateTeamDto createTeam, string createdBy)
        {
            if (createTeam == null)
            {
                throw new ArgumentNullException(nameof(createTeam), "Invalid organisation data");
            }

            // Check existence of Team
            var departmentExists = await _context.Departments.AnyAsync(c => c.DepartmentId == departmentId);
            if (!departmentExists)
            {
                throw new ArgumentException("Invalid departmentId");
            }

            var contactExists = await _context.Contacts.AnyAsync(c => c.ContactId == contactId);
            if (!contactExists)
            {
                throw new ArgumentException("Invalid contactId");
            }

            Address addressEntity = null;
            if (createTeam.GetAddressFrom == "Organisation")
            {

                addressEntity = await _addressService.GetAddressOfOrganisationByDepartment(departmentId);
            }
            else
            {
                addressEntity = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.PostCode == createTeam.Address.PostCode);
            }

            if (addressEntity == null)
            {
                // Tạo mới địa chỉ nếu không tồn tại
                addressEntity = await _addressService.HandleAddressAsync(createTeam.Address, createdBy);
            }
            CompanyContact companyContact = null;
            var companyContactByOrganisation = await _companyContactService.GetCompanyContactOrganisationByDepartment(departmentId);
            if (createTeam.CompanyContact.TypeOfBusiness.IsNullOrEmpty() ||
                createTeam.CompanyContact.SICCode.IsNullOrEmpty() ||
                createTeam.CompanyContact.WebAddress.IsNullOrEmpty())
            {
                //companyContact = new CompanyContact {
                createTeam.CompanyContact.TypeOfBusiness = companyContactByOrganisation.TypeOfBusiness;
                createTeam.CompanyContact.SICCode = companyContactByOrganisation.SICCode;
                createTeam.CompanyContact.WebAddress = companyContactByOrganisation.WebAddress;
                if (
                    createTeam.CompanyContact.FullDescription != companyContactByOrganisation.FullDescription
                    || createTeam.CompanyContact.PhoneNumber != companyContactByOrganisation.PhoneNumber
                    || createTeam.CompanyContact.Fax != companyContactByOrganisation.Fax
                    || createTeam.CompanyContact.Email != companyContactByOrganisation.Email
                    || createTeam.CompanyContact.CharityNumber != companyContactByOrganisation.CharityNumber
                    || createTeam.CompanyContact.CompanyNumber != companyContactByOrganisation.CompanyNumber
                    )
                {
                    companyContact = new CompanyContact
                    {
                        TypeOfBusiness = createTeam.CompanyContact.TypeOfBusiness,
                        SICCode = createTeam.CompanyContact.SICCode,
                        WebAddress = createTeam.CompanyContact.WebAddress,
                        FullDescription = createTeam.CompanyContact.FullDescription,
                        PhoneNumber = createTeam.CompanyContact.PhoneNumber,
                        Fax = createTeam.CompanyContact.Fax,
                        Email = createTeam.CompanyContact.Email,
                        CharityNumber = createTeam.CompanyContact.CharityNumber,
                        CompanyNumber = createTeam.CompanyContact.CompanyNumber,
                        CreatedBy = createdBy,
                        CreatedDate = DateTime.Now
                    };
                }
                else companyContact = await _context.CompanyContacts.Where(cc => cc.SICCode == companyContactByOrganisation.SICCode).FirstOrDefaultAsync();
            }
            
            //companyContact = _mapper.Map<CompanyContact>(createTeam.CompanyContact);
            //}
            else
            {
                companyContact = await _context.CompanyContacts.Where(cc => cc.PhoneNumber == createTeam.CompanyContact.PhoneNumber).FirstOrDefaultAsync();
                if (companyContact == null)
                {
                    var companyContactEntity = await _companyContactService.HandleCompanyContactAsync(createTeam.CompanyContact, createdBy);
                    companyContact = companyContactEntity;
                }
                
            }
            //var companyContactEntity = await _companyContactService.HandleCompanyContactAsync(createTeam.CompanyContact, createdBy);

            var teamMap = _mapper.Map<Team>(createTeam);
            teamMap.DepartmentId = departmentId;
            teamMap.ContactId = contactId;
            teamMap.Address = addressEntity;
            teamMap.CompanyContact = companyContact;
            teamMap.CreatedBy = createdBy;
            teamMap.CreatedDate = DateTime.Now;
            teamMap.IsActive = false;

            _context.Teams.Add(teamMap);

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> ChangeTeamIsActiveAsync(int teamId, bool isActive)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
            {
                return false;
            }

            team.IsActive = !team.IsActive;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTeamAsync(int departmentId, int contactId, UpdateTeamDto updateTeam,
            string updatedBy)
        {
            if (updateTeam == null)
            {
                throw new ArgumentNullException(nameof(updateTeam), "Invalid team data");
            }

            // Check existence of Organisation
            var departmentExists = await _context.Departments.AnyAsync(c => c.DepartmentId == departmentId);
            if (!departmentExists)
            {
                throw new ArgumentException("Invalid organisationId");
            }

            // Check existence of Contact
            var contactExists = await _context.Contacts.AnyAsync(c => c.ContactId == contactId);
            if (!contactExists)
            {
                throw new ArgumentException("Invalid contactId");
            }

            // Retrieve the existing Team
            var team = await _context.Teams
                .Include(t => t.Address)
                .Include(t => t.CompanyContact)
                .FirstOrDefaultAsync(t => t.TeamId == updateTeam.TeamId);

            if (team == null)
            {
                throw new ArgumentException("Team not found");
            }

            // Handle Address
            Address addressEntity = null;
            if (updateTeam.GetAddressFrom == "Organisation")
            {
                addressEntity = await _addressService.GetAddressOfOrganisationByDepartment(departmentId);
            }
            else
            {
                addressEntity = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.PostCode == updateTeam.Address.PostCode);
            }

            if (addressEntity == null)
            {
                // Create new address if it doesn't exist
                addressEntity = _mapper.Map<Address>(updateTeam.Address);
                if (string.IsNullOrEmpty(addressEntity.PostCode))
                {
                    throw new ArgumentException("Address information is missing or invalid",
                        nameof(updateTeam.Address));
                }

                addressEntity.CreatedBy = updatedBy;
                addressEntity.CreatedDate = DateTime.Now;
                _context.Addresses.Add(addressEntity);
                await _context.SaveChangesAsync(); // Save the address before proceeding
            }
            else
            {
                // Update existing address
                addressEntity.UpdatedBy = updatedBy;
                addressEntity.UpdatedDate = DateTime.Now;
                _context.Addresses.Update(addressEntity);
                await _context.SaveChangesAsync(); // Save the address before proceeding
            }

            // Handle CompanyContact
            CompanyContact companyContact;
            if (updateTeam.CompanyContact == null ||
                string.IsNullOrEmpty(updateTeam.CompanyContact.TypeOfBusiness) ||
                string.IsNullOrEmpty(updateTeam.CompanyContact.SICCode) ||
                string.IsNullOrEmpty(updateTeam.CompanyContact.WebAddress))
            {
                companyContact = await _companyContactService.GetCompanyContactByOrganisationId(departmentId);
            }
            else
            {
                companyContact = await _context.CompanyContacts
                    .FirstOrDefaultAsync(cc => cc.PhoneNumber == updateTeam.CompanyContact.PhoneNumber);

                if (companyContact == null)
                {
                    companyContact = _mapper.Map<CompanyContact>(updateTeam.CompanyContact);
                    companyContact.CreatedBy = updatedBy;
                    companyContact.CreatedDate = DateTime.Now;
                    _context.CompanyContacts.Add(companyContact);
                }
                else
                {
                    companyContact.UpdatedBy = updatedBy;
                    companyContact.UpdatedDate = DateTime.Now;
                    _context.CompanyContacts.Update(companyContact);
                }

                await _context.SaveChangesAsync(); // Save the company contact before proceeding
            }


            // Update Team properties
            team.Name = updateTeam.Name;
            team.ShortDescription = updateTeam.ShortDescription;
            team.LeadContact = updateTeam.LeadContact;
            team.Address = addressEntity;
            team.CompanyContact = companyContact;
            team.UpdatedBy = updatedBy;
            team.UpdatedDate = DateTime.Now;
            team.IsActive = updateTeam.IsActive;

            _context.Teams.Update(team);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
