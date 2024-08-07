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
    public interface IAddressService
    {
        Task<List<string>> GetListPostCode();
        Task<Address> HandleAddressAsync(AddressDto addressDto, string createdBy);
        Task<Address> GetAddressByDirectorate(int directorateId);
        Task<Address> GetAddressByOrganisation(int directorateId);
        Task<Address> GetAddressByOrganisationId(int organisationId);
        Task<Address> GetAddressByDepartment(int departmentId);
        Task<Address> GetAddressOfOrganisationByDepartment(int departmentId);
        Task<List<string>> GetAllPostCode();
        Task<bool> CheckPostCode(AddressDto address);
        Task<AddressDto?> AddAddress(AddressDto address, string createdBy);
        Task<AddressDto?> UpdateAddress(UpdateAddressModel updateAddressModel, string updatedBy);

    }
}
