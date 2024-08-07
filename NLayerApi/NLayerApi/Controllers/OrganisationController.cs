using BusinessLayer.Interfaces;
using Common.Helper;
//using Common.Helper;
using BusinessLayer.Extensions;
using BusinessLayer.Services;
using CommonWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using CommonWeb.Models;
using CommonWeb.Dto;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganisationsController : ControllerBase
    {
        private readonly IOrganisationService _organisationService;
        private readonly GeoLocationService _geoLocationService;
        private readonly IRegionService _regionService;
        public OrganisationsController(IOrganisationService organisationService, GeoLocationService geoLocationService, IRegionService regionService)
        {
            _organisationService = organisationService;
            _organisationService = organisationService;
            _geoLocationService = geoLocationService;
            _regionService = regionService;

        }

        [HttpGet]
        public async Task<ActionResult<PagedList<OrganisationDto>>> GetOrganisations([FromQuery] OrganisationParams organisationParams)
        {
            var organisations = await _organisationService.GetOrganisationAsync(organisationParams);

            Response.AddPaginationHeader(organisations.MetaData);

            return Ok(organisations);
        }

        [Authorize]
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateOrganisation(int id)
        {
            try
            {
                var updatedBy = User.Identity.Name;
                var result = await _organisationService.ActivateOrganisationAsync(id, updatedBy);

                if (!result)
                {
                    return NotFound("Organisation not found.");
                }

                return Ok("Organisation has been activated.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("getLeadContact")]
        //public async Task<IActionResult> GetLeadContact()
        //{
        //    var leadContacts=await _organisationService.GetLeadContact();
        //    return Ok(new { leadContacts });
        //}

        //[HttpGet("getAddress")]
        //public async Task<IActionResult> GetAddress()
        //{
        //    var addresses=await _organisationService.GetAddress();
        //    return Ok(new { addresses });
        //}

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateOrganisation(int contactId, [FromBody] CreateOrganisationDto createOrganisationDto)
        {
            if (createOrganisationDto == null)
                return BadRequest(ModelState);

            var createdBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");

            try
            {
                var success = await _organisationService.CreateOrganisationAsync(contactId,createOrganisationDto, createdBy);

                if (success)
                {
                    return Ok("Create successfully");
                }
                else
                {
                    return BadRequest(new ProblemDetails { Title = "Problem creating new organisation" });
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(422, ModelState);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "An unexpected error occurred", Detail = ex.Message });
            }
        }
        //[HttpGet("get premises")]
        //public async Task<ActionResult<List<GetOrganisationPremisesModel>>> GetOrganisationPremises(int id)
        //{
            
        //    List<GetOrganisationPremisesModel> premisesModels = await _organisationService.GetOrganisationPremisesAsync(id);
            
        //    return Ok(premisesModels);
        //}

        //[HttpGet("Get LocatedIn Info")]
        //public async Task<ActionResult<GetGeoLocationModel>> GetLocatedIn(int postId)
        //{
        //    var geoLocationInfo =await _geoLocationService.GetGeoLocation(postId);
        //    return geoLocationInfo;
        //}

        //[HttpGet("Get Government Office Region")]
        //public async Task<ActionResult<List<GetOrganisationGORModel>>> GetGOR(int countyId)
        //{
        //    var organisationGor = await _regionService.GetOrganisationGOR(countyId);

        //    return Ok(organisationGor);
        //}

        [HttpPut("{id}/IsActive")]
        public async Task<IActionResult> ChangeIsActive(int id, [FromBody] bool isActive)
        {
            var result = await _organisationService.ChangeOgranisationIsActiveAsync(id, isActive);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }

    }
}

