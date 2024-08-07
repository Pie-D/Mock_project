using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Common.Models;
using CommonWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PremiseController : ControllerBase
    {
        private readonly GeoLocationService _geoLocationService;

        private readonly IPremisesService _premisesService;

        private readonly IGovOfficeRegionService _govOfficeRegionService;

        private readonly ITrustDistrictService _trustDistrictService;

        private readonly ITrustRegionService _trustRegionService;

        public PremiseController(GeoLocationService geoLocationService, IPremisesService premisesService, IGovOfficeRegionService govOfficeRegionService, ITrustDistrictService trustDistrictService, ITrustRegionService trustRegionService)
        {
            _geoLocationService = geoLocationService;
            _premisesService = premisesService;
            _govOfficeRegionService = govOfficeRegionService;
            _trustDistrictService = trustDistrictService;
            _trustRegionService = trustRegionService;
        }

        [Authorize]
        [HttpGet("Get LocatedIn Info")]
        public async Task<ActionResult<GetGeoLocationModel>> GetLocatedIn(string postId)
        {
            GetGeoLocationModel? geoLocationInfo = null;
            try
            {
                geoLocationInfo = await _geoLocationService.GetGeoLocation(postId);
                return Ok(geoLocationInfo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Authorize]
        [HttpGet("GetGovOfficeRegions{id}")]
        public async Task<ActionResult> GetGovOfficeRegions(int id)
        {
            var govOffRegs = await _govOfficeRegionService.GetGovOfficeRegionsByCountyId(id);
            return Ok(govOffRegs);
        }

        [Authorize]
        [HttpGet("GetTrustRegions")]
        public async Task<ActionResult> GetTrustRegion()
        {
            var trustRegions = await _trustRegionService.GetTrustRegions();
            return Ok(trustRegions);
        }

        [Authorize]
        [HttpGet("GetTrustDistrict")]
        public async Task<ActionResult> GetTrustDistrictByTrustRegionId(int trustRegionId)
        {
            if (await _trustRegionService.TrustRegionExists(trustRegionId))
            {
                var trustDistricts = await _trustDistrictService.GetTrustDistrictByTrustRegionId(trustRegionId);
                return Ok(trustDistricts);
            }
            else
            {
                return BadRequest("Trust Region Id invalid");
            }

        }

        [Authorize]
        [HttpGet("GetPremises")]
        public async Task<ActionResult> GetOrgPremises(int Id)
        {

            var result = await _premisesService.GetPremisesByOrgId(Id);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("UpdatePremises")]
        public async Task<ActionResult> UpdatePremises(List<UpdatePremiseModel?> updatePremiseModels)
        {
            var result = await _premisesService.UpdatePremises(updatePremiseModels);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(updatePremiseModels);
        }

    }
}
