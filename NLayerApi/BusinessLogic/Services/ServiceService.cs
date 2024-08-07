using AutoMapper;
using BusinessLayer.Interfaces;
using Common.Dto;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    //public class ServiceService(DataContext dataContext, IMapper mapper) : IServiceService
    //{
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public ServiceService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ServiceDto>> GetAllService()
        {
            return await _context.Services
                .Where(s => s.IsActive)
                .Select(s => new ServiceDto
                {
                    ServiceName = s.ServiceName,
                    ServiceId = s.ServiceId
                }).ToListAsync();
        }

    }
}
