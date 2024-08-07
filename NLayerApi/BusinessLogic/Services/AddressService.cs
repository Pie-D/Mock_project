using AutoMapper;
using BusinessLayer.Interfaces;
using Common.Models;
using CommonWeb.Dto;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        //private readonly IOrganisationService _orgaService;
        public AddressService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            //_orgaService = orgaService;
        }
        public async Task<List<string>> GetListPostCode()
        {
            var listPostCode=await _context.Addresses.Select(pc=>pc.PostCode).ToListAsync();
            return listPostCode;
        }

        public async Task<Address> HandleAddressAsync(AddressDto addressDto, string createdBy)
        {
                var address = _mapper.Map<Address>(addressDto);
                if (string.IsNullOrEmpty(address.PostCode))
                {
                    throw new ArgumentException("Address information is missing or invalid.");
                }

                address.CreatedBy = createdBy;
                address.CreatedDate = DateTime.Now;
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

            return address;
        }

        public async Task<Address> GetAddressByDirectorate(int directorateId)
        {
            var directorate =await _context.Directorates.FirstOrDefaultAsync(x => x.DirectorateId == directorateId);
            var address =await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == directorate.AddressId);
            //if (address == null)
            //{

            //}
            return address;
        }

        public async Task<Address> GetAddressByOrganisation(int directorateId)
        {
            var directorate = await _context.Directorates
    .FirstOrDefaultAsync(x => x.DirectorateId == directorateId);

            if (directorate == null)
                return null;

            var organisation = await _context.Organisations
                .FirstOrDefaultAsync(o => o.OrganisationId == directorate.OrganisationId);

            if (organisation == null)
            {
                return null; // Or handle the case when organisation is not found
            }

            var address =await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == organisation.AddressId);
            return address;
        }

        public async Task<Address> GetAddressByOrganisationId(int organisationId)
        {
            var organisation = await _context.Organisations
                .FirstOrDefaultAsync(o => o.OrganisationId == organisationId);

            if (organisation == null)
            {
                return null; 
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == organisation.AddressId);
            return address;
        }

        public async Task<Address> GetAddressByDepartment(int departmentId)
        {
            var department=await _context.Departments.FirstOrDefaultAsync(d=>d.DepartmentId == departmentId);

            if (department == null)
            {
                return null;
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == department.AddressId);
            return address;
        }

        public async Task<Address> GetAddressOfOrganisationByDepartment(int departmentId)
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

            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == organisation.AddressId);
            return address;
        }

        public async Task<List<string>> GetAllPostCode()
        {
            var listAddress = await _context.Addresses.Select(c => c.PostCode).ToListAsync();
            return listAddress;
        }

        public async Task<bool> CheckPostCode(AddressDto address)
        {
            var addressCheck = await _context.Addresses.FirstOrDefaultAsync(p => p.PostCode == address.PostCode);
            return addressCheck != null && addressCheck.TownId == address.TownId;
        }

        public async Task<AddressDto?> AddAddress(AddressDto address, string createdBy)
        {
            if (!await CheckPostCode(address)) return null;
            var addressMap = _mapper.Map<Address>(address);
            addressMap.CreatedBy = createdBy;
            addressMap.CreatedDate = DateTime.Now;
            await _context.Addresses.AddAsync(addressMap);
            var result = await _context.SaveChangesAsync() > 0;
            var addressDto = _mapper.Map<AddressDto>(addressMap);
            return result ? addressDto : null;
        }

        public async Task<AddressDto?> UpdateAddress(UpdateAddressModel updateAddressModel, string createdBy)
        {
            var address = updateAddressModel.AddressDto;
            var addressId = updateAddressModel.AddressIdUpdate;
            if (!await CheckPostCode(address)) return null;
            var addressFind = await _context.Addresses.FirstOrDefaultAsync(p => p.AddressId == addressId);
            if (addressFind == null)
            {
                return null;
            }

            addressFind.Address1 = address.Address1;
            addressFind.Address2 = address.Address2;
            addressFind.Address3 = address.Address3;
            addressFind.PostCode = address.PostCode;
            addressFind.City = address.City;
            addressFind.TownId = address.TownId;
            addressFind.UpdatedBy = createdBy;
            addressFind.UpdatedDate = DateTime.Now;
            _context.Addresses.Update(addressFind);
            var result = await _context.SaveChangesAsync() > 0;
            var addressDto = _mapper.Map<AddressDto>(addressFind);
            return result ? addressDto : null;
        }

    }
}