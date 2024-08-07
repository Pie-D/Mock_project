using BusinessLayer.Interfaces;
using Common.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Extensions;
using CommonWeb.Dto;
using BusinessLayer.Services;
using Common.Models;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;

        }
        [HttpGet]
        public async Task<ActionResult<PagedList<DepartmentDto>>> GetDepartments([FromQuery] DepartmentParams departmentParams)
        {
            var departments = await _departmentService.GetDepartmentAsync(departmentParams);

            Response.AddPaginationHeader(departments.MetaData);

            return Ok(departments);
        }
        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> AddDepartment(int directorateId, int contactId, [FromBody] CreateDepartmentDto createDepartment)
        //{
        //    if (createDepartment == null)
        //        return BadRequest(ModelState);

        //    var createdBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");

        //    //var result = await _departmentService.AddDepartmentAsync(directorateId, contactId, createDepartment, User.Identity.Name);

        //    try
        //    {
        //        var success = await _departmentService.AddDepartmentAsync(directorateId, contactId, createDepartment, createdBy);

        //        if (success)
        //        {
        //            return Ok("Create successfully");
        //        }
        //        else
        //        {
        //            return BadRequest(new ProblemDetails { Title = "Problem creating new department" });
        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(new ProblemDetails { Title = ex.Message });
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        return StatusCode(422, ModelState);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ProblemDetails { Title = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ProblemDetails { Title = "An unexpected error occurred", Detail = ex.Message });
        //    }
        //}
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentModel createDepartmentModel)
        {
            var createdBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");

            var department = await _departmentService.CreateDepartment(createDepartmentModel, createdBy);
            return Ok(department);

        }



        [Authorize]
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateDepartment(int id)
        {
            try
            {
                var updatedBy = User.Identity.Name;
                var result = await _departmentService.ActivateDepartmentAsync(id, updatedBy);

                if (!result)
                {
                    return NotFound("Department not found.");
                }

                return Ok("Department has been activated.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/IsActive")]
        public async Task<IActionResult> ChangeIsActive(int id, [FromBody] bool isActive)
        {
            var result = await _departmentService.ChangeDepartmentIsActiveAsync(id, isActive);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPut]
        public async Task<ActionResult<DepartmentDto>> UpdateDepartment(UpdateDepartmentModel updateDepartmentModel)
        {
            var updatedBy = User.Identity?.Name ?? throw new InvalidOperationException("User identity is not available.");

            var department = await _departmentService.UpdateDepartment(updateDepartmentModel, updatedBy);
            return Ok(department);
        }
    }
}
