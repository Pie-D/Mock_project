using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //public class ServiceController(IServiceService serviceService, IMapper mapper) : ControllerBase
    //{
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetListService()
        {
            var listService = await _serviceService.GetAllService();

            return Ok(listService.ToJson());
        }

    }
}
