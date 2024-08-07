using AutoMapper;
using BusinessLayer.Interfaces;
using CommonWeb.Models;
using DataAccess.Entities;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class TrustRegionService : ITrustRegionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TrustRegionService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetTrustRegionModel>> GetTrustRegions()
        {
            var trustRegions = await _context.TrustRegions.ToListAsync();

            var trustRegionModels = _mapper.Map<List<GetTrustRegionModel>>(trustRegions);

            return trustRegionModels;
        }


        public async Task<bool> AddTrustRegion(GetTrustRegionModel trustRegionModel)
        {
            var trustRegion = _mapper.Map<TrustRegion>(trustRegionModel);
            await _context.TrustRegions.AddAsync(trustRegion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TrustRegionExists(int id)
        {
            return await _context.TrustRegions.AnyAsync(s => s.TrustRegionId == id);
        }
    }
}
