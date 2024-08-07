using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Common.Dto;
using Common.Models;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NLayerApi.Controllers
{
    //public class OrganisationProgrammeController : Controller
    //{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class OrganisationProgrammeController(IOrganisationProgrammeService organisationProgrammeService, IMapper mapper) : ControllerBase
    //{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganisationProgrammeController : Controller
    {
        private readonly IOrganisationProgrammeService _organisationProgrammeService;
        private readonly DataContext _context;

        public OrganisationProgrammeController(IOrganisationProgrammeService organisationProgrammeService, DataContext context)
        {
            _organisationProgrammeService = organisationProgrammeService;
            _context = context;
        }

        [Authorize]
        [HttpPost("")]
        public async Task<ActionResult<DataAccess.Entities.OrganisationProgramme?>> CreateOrganisationProgramme(
        CreateOrganisationProgrammeModel organisationProgrammesDto)
        {
            var createdBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");
            return Ok(await _organisationProgrammeService.CreateOrganisationProgramme(organisationProgrammesDto, createdBy));
        }
    }
}
