using AutoMapper;
using BusinessLayer.Interfaces;
using CommonWeb.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services;

public class RegionService : IRegionService
{
    private DataContext _dataContext;
    private IMapper _mapper;
    public RegionService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<List<GetOrganisationGORModel>> GetOrganisationGOR(int countyId)
    {

        var gors = await _dataContext.GovOfficeRegions
            .Where(s => s.CountyId == countyId)
            .ToListAsync();

        var gorsModels = _mapper.Map<List<GetOrganisationGORModel>>(gors);

        return gorsModels;
    }
    
    
}