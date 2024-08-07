using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Common.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace NLayerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //public class ProgrammeController : ControllerBase
    //{
    //    public class ProgrammeController(IProgrammeService programmeService)
    //    {
    public class ProgrammeController : Controller
    {
        private readonly IProgrammeService _programmeService;
        public ProgrammeController(IProgrammeService programmeService)
        {
            _programmeService = programmeService;
        }
        [HttpGet("")]
            public async Task<ActionResult<IEnumerable<ProgrammeDto>>> GetAllProgramme()
            {
                var programmeList = await _programmeService.GetAllProgramme();
                return Ok(programmeList.ToJson());
            }
        }
    }
