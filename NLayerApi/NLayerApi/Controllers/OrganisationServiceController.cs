using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Common.Dto;
using Common.Models;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //public class OrganisationServiceController(IOrganisationServiceService organisationServiceService, IMapper mapper) : ControllerBase
    //{
    public class OrganisationServiceController : Controller
    {
        private readonly IOrganisationServiceService _organisationServiceService;
        private readonly DataContext _context;

        public OrganisationServiceController(IOrganisationServiceService organisationServiceService)
        {
            _organisationServiceService = organisationServiceService;
        }
        [Authorize]
        [HttpPost("")]
        public async Task<ActionResult<IEnumerable<OrganisationServiceDto>?>> CreateOrganisationService(
    CreateOrganisationServiceModel organisationServiceDto)
        {
            var createdBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");
            return Ok(await _organisationServiceService.CreateOrganisationService(organisationServiceDto, createdBy));
        }
    }
}
