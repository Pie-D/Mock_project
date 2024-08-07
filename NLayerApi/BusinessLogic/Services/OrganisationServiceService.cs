using AutoMapper;
using BusinessLayer.Interfaces;
using Common.Dto;
using Common.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class OrganisationServiceService : IOrganisationServiceService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        //private readonly IOrganisationService _orgaService;
        public OrganisationServiceService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            //_orgaService = orgaService;
        }
        //public class OrganisationServiceService(DataContext dataContext, IMapper mapper) : IOrganisationServiceService
        //{
            public async Task<IEnumerable<OrganisationServiceDto>?> CreateOrganisationService(CreateOrganisationServiceModel organisationService, string createdBy)
            {
                var organisationServicesDto = organisationService.OrganisationServicesDto;
                var organisationServiceMap = _mapper.Map<IEnumerable<DataAccess.Entities.OrganisationService>>(organisationServicesDto).ToList();
                foreach (var item in organisationServiceMap)
                {
                    item.CreatedBy = createdBy;
                    item.CreatedDate = DateTime.Now;
                }
                await _context.OrganisationServices.AddRangeAsync(organisationServiceMap);
                var result = await _context.SaveChangesAsync() > 0;
                return result ? organisationServicesDto : null;
            }
        }
    }
