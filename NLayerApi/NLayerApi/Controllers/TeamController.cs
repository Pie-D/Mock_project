using BusinessLayer.Interfaces;
using CommonWeb.Dto;
using Common.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Extensions;
using BusinessLayer.Services;
using Common.Dto;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;

        }
        [HttpGet]
        public async Task<ActionResult<PagedList<TeamDto>>> GetTeams([FromQuery] TeamParams teamParams)
        {
            var teams = await _teamService.GetTeamAsync(teamParams);

            Response.AddPaginationHeader(teams.MetaData);

            return Ok(teams);
        }

        [Authorize]
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateTeam(int id)
        {
            try
            {
                var updatedBy = User.Identity.Name;
                var result = await _teamService.ActivateTeamAsync(id, updatedBy);

                if (!result)
                {
                    return NotFound("Team not found.");
                }

                return Ok("Team has been activated.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTeam(int departmentId, int contactId, [FromBody] CreateTeamDto createTeam)
        {
            if (createTeam == null)
                return BadRequest(ModelState);

            var createdBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");

            //var result = await _teamService.AddTeamAsync(teamId, contactId, createTeam, User.Identity.Name);

            try
            {
                var success = await _teamService.AddTeamAsync(departmentId, contactId, createTeam, createdBy);

                if (success)
                {
                    return Ok("Create successfully");
                }
                else
                {
                    return BadRequest(new ProblemDetails { Title = "Problem creating new team" });
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
            var result = await _teamService.ChangeTeamIsActiveAsync(id, isActive);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateTeam(int departmentId, int contactId, [FromBody] UpdateTeamDto updateTeam)
        {
            if (updateTeam == null)
                return BadRequest(ModelState);

            var updatedBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");

            try
            {
                var success = await _teamService.UpdateTeamAsync(departmentId, contactId, updateTeam, updatedBy);

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
    }
}
