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
    public class TrustDistrictService : ITrustDistrictService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TrustDistrictService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetTrustDistrictModel>> GetTrustDistrictByTrustRegionId(int id)
        {
            var trustDistricts = _context.TrustDistricts
                .Where(t => t.TrustRegionId == id).ToList()
                ;

            List<GetTrustDistrictModel> trustDistrictModels = _mapper.Map<List<GetTrustDistrictModel>>(trustDistricts);

            return trustDistrictModels;
        }

        public async Task<bool> AddTrustDistrict(GetTrustDistrictModel trustDistrictModel)
        {
            var trustDistrict = _mapper.Map<TrustDistrict>(trustDistrictModel);
            await _context.TrustDistricts.AddAsync(trustDistrict);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TrustDistrictExist(int id)
        {
            return await _context.TrustDistricts.AnyAsync(t => t.TrustDistricId == id);
        }
    }
}
