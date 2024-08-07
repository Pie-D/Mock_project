using AutoMapper;
using BusinessLayer.Interfaces;
using Common.Custom;
using Common.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services;

public class PremisesService : IPremisesService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PremisesService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<GetPremiseModel>> GetPremisesByOrgId(int id)
    {
        if (!await _context.Organisations.AnyAsync(s => s.OrganisationId == id))
        {
            throw new ArgumentException("Organisation Id not valid");
        }
        //Get list of Organisation's Services 
        var serviceIds = await _context.OrganisationServices
            .Where(s => s.OrganisationId == id)
            .Select(s => s.ServiceId)
            .ToListAsync();

        //Get list or premises link to those services
        var premises = await _context.Premises
            .Where(s => serviceIds.Contains(s.ServiceId))
            .Include(p => p.CompanyContact)
            .Include(s => s.Address)
            .ThenInclude(a => a.Town)
            .Include(s => s.Service)
            .ToListAsync();

        var premisesModels = _mapper.Map<List<GetPremiseModel>>(premises);

        return premisesModels;
    }

    public async Task<Result> UpdatePremise(UpdatePremiseModel? model)
    {
        if (model == null) return Result.Failure(DocumentedErrors.NullInvalid);
        //validate premiseId
        if (!await PremiseExists(model.Premise.PremiseId))
        {
            return Result.Failure(DocumentedErrors.InvalidPremiseId);
        }

        if (model.Premise.LocationName == null)
        {
            return Result.Failure(DocumentedErrors.NullInvalid);
        }

        if (model.Premise.CompanyContact.PhoneNumber == null)
        {
            return Result.Failure(DocumentedErrors.UpdateFailedAtEntry(model.Premise.LocationName));
        }

        if (model.Premise.CompanyContact.CompanyContactId == null)
        {
            return Result.Failure(DocumentedErrors.InvalidContactId);
        }

        if (model.Premise.Address.AddressId == null)
        {
            return Result.Failure(DocumentedErrors.InvalidAddressId);
        }
        //check companycontactid
        if (!await _context.CompanyContacts.AnyAsync(s =>
                s.CompanyContactId == model.Premise.CompanyContact.CompanyContactId))
        {
            return Result.Failure(DocumentedErrors.UpdateFailedAtEntry(model.Premise.LocationName));
        }
        //Update premise name
        var premise = await _context.Premises.FirstOrDefaultAsync(s => s.PremiseId == model.Premise.PremiseId);
        if (premise.LocationName != model.Premise.LocationName || premise.PrimaryLocation != model.Premise.PrimaryLocation)
        {
            premise.LocationName = model.Premise.LocationName;
            _context.Premises.Update(premise);
        }

        //update address line
        var address = await _context.Addresses.FirstOrDefaultAsync(s => s.AddressId == model.Premise.AddressId);
        if (address.Address1 != model.Premise.Address.Address1)
        {
            address.Address1 = model.Premise.Address.Address1;

            _context.Addresses.Update(address);
        }

        //update phone number
        var companycontact = await _context.CompanyContacts
               .FirstOrDefaultAsync(s => s.CompanyContactId == model.Premise.CompanyContact.CompanyContactId);
        if (companycontact.PhoneNumber != model.Premise.CompanyContact.PhoneNumber)
        {
            companycontact.PhoneNumber = model.Premise.CompanyContact.PhoneNumber;
            _context.CompanyContacts.Update(companycontact);
        }

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdatePremises(List<UpdatePremiseModel?> premiseModels)
    {
        foreach (var premiseModel in premiseModels)
        {
            var result = await UpdatePremise(premiseModel);
            if (!result.IsSuccess)
            {
                return result;
            }

        }
        return Result.Success();
    }

    public async Task<bool> PremiseExists(int id)
    {
        return await _context.Premises.AnyAsync(s => s.PremiseId == id);
    }
}