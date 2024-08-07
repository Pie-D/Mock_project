using BusinessLayer.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
            
        }

        [HttpGet("listPostCode")]
        public async Task<IActionResult> GetPostCode()
        {
            var listPostCode = await _addressService.GetListPostCode();
            //.Distinct() : tại mỗi khu vực có mã riêng mà, cần gì

            return Ok(new { listPostCode });
        }

        //[HttpGet("GetAllPostCode")]
        //public async Task<IActionResult> GetAPostCode()
        //{
        //    var listPostCode = await _addressService.GetListPostCode();
        //    //.Distinct() : tại mỗi khu vực có mã riêng mà, cần gì

        //    return Ok(new { listPostCode });
        //}
    }
}
