using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Common.Helper;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Extensions;
using CommonWeb.Dto;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorateController : Controller
    {
        private readonly IDirectorateService _directorateService;
        public DirectorateController(IDirectorateService directorateService)
        {
            _directorateService = directorateService;

        }
        [HttpGet]
        public async Task<ActionResult<PagedList<DirectorateDto>>> GetDirectorates([FromQuery] DirectorateParams directorateParams)
        {
            var directorates = await _directorateService.GetDirectorateAsync(directorateParams);

            Response.AddPaginationHeader(directorates.MetaData);

            return Ok(directorates);
        }

        [Authorize]
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateDirectorate(int id)
        {
            try
            {
                var updatedBy = User.Identity.Name;
                var result = await _directorateService.ActivateDirectorateAsync(id, updatedBy);

                if (!result)
                {
                    return NotFound("Directorate not found.");
                }

                return Ok("Directorate has been activated.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddDirectorate(int organisationId, int contactId, [FromBody] CreateDirectorateDto createDirectorate)
        {
            if (createDirectorate == null)
                return BadRequest(ModelState);

            var createdBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");

            //var result = await _directorateService.AddDirectorateAsync(directorateId, contactId, createDirectorate, User.Identity.Name);

            try
            {
                var success = await _directorateService.AddDirectorateAsync(organisationId, contactId, createDirectorate, createdBy);

                if (success)
                {
                    return Ok("Create successfully");
                }
                else
                {
                    return BadRequest(new ProblemDetails { Title = "Problem creating new directorate" });
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

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateDirectorate(int organisationId, int contactId, [FromBody] UpdateDirectorateDto updateDirectorate)
        {
            if (updateDirectorate == null)
                return BadRequest(ModelState);

            var updatedBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");

            try
            {
                var success = await _directorateService.UpdateDirectorateAsync(organisationId, contactId, updateDirectorate, updatedBy);

                if (success)
                {
                    return Ok("Update successfully");
                }
                else
                {
                    return BadRequest(new ProblemDetails { Title = "Problem updating directorate" });
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

        [HttpPut("{id}/IsActive")]
        public async Task<IActionResult> ChangeIsActive(int id, [FromBody] bool isActive)
        {
            var result = await _directorateService.ChangeDirectorateIsActiveAsync(id, isActive);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }

    }
}
