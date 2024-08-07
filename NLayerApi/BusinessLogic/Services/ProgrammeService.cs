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
    public class ProgrammeService : IProgrammeService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public ProgrammeService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //public class ProgrammeService(DataContext dataContext) : IProgrammeService
        //{
        public async Task<IEnumerable<ProgrammeDto>> GetAllProgramme()
        {
            return await _context.Programmes
               .Where(s => s.IsActive)
               .Select(s => new ProgrammeDto
               {
                   ProgrammeName = s.ProgrammeName,
                   ProgrammeId = s.ProgrammeId
               }).ToListAsync();
        }
    }
}
