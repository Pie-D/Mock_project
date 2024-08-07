using AutoMapper;
using BusinessLayer.Interfaces;
using Common.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class GovOfficeRegionService : IGovOfficeRegionService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GovOfficeRegionService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<GetGovOfficeRegionModel>> GetGovOfficeRegionsByCountyId(int id)
        {
            var govOfficeRegions = await _dataContext.GovOfficeRegions
                .Where(s => s.CountyId == id)
                .AsNoTracking()
                .ToListAsync();

            var govOfficeRegionModels = _mapper.Map<List<GetGovOfficeRegionModel>>(govOfficeRegions);

            return govOfficeRegionModels;
        }
    }
}
