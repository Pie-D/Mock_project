using BusinessLayer.Interfaces;
using CommonWeb.Dto;
using Common.Helper;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Common.Dto;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportingMaterialController : Controller
    {
        private readonly ISupportingMaterialService _supportingMaterialService;
        public SupportingMaterialController(ISupportingMaterialService supportingMaterialService)
        {
            _supportingMaterialService = supportingMaterialService;

        }
        [HttpGet]
        public async Task<ActionResult<PagedList<SupportingMaterialDto>>> GetSupportingMaterials([FromQuery] SupportingMaterialParams supportingMaterialParams)
        {
            var supportingMaterials = await _supportingMaterialService.GetSupportingMaterialAsync(supportingMaterialParams);

            Response.AddPaginationHeader(supportingMaterials.MetaData);

            return Ok(supportingMaterials);
        }


        [Authorize]
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateSupportingMaterial(int id)
        {
            try
            {
                var updatedBy = User.Identity.Name;
                var result = await _supportingMaterialService.ActivateSupportingMaterialAsync(id, updatedBy);

                if (!result)
                {
                    return NotFound("SupportingMaterial not found.");
                }

                return Ok("SupportingMaterial has been activated.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateSupportingMaterial(int materialId, [FromBody] UpdateSupporttingMaterial updateSupporttingMaterial)
        {
            if (updateSupporttingMaterial == null)
                return BadRequest(ModelState);
            var updatedBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");
            try
            {
                var success = await _supportingMaterialService.UpdateSupportingMaterialAsync(materialId, updateSupporttingMaterial, updatedBy);

                if (success)
                {
                    return Ok("Update successfully");
                }
                else
                {
                    return BadRequest(new ProblemDetails { Title = "Problem updating supporting material" });
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
    }
}