using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NLayerApi.Controllers
{
    public class CompanyContactController : Controller
    {
        private readonly ICompanyContactService _companyContactService;
        public CompanyContactController(ICompanyContactService companyContactService)
        {
            _companyContactService = companyContactService;
        }

        [HttpGet("GetTypeOfBusinesses")]
        public async Task<IActionResult> GetTypeOfBusinesses()
        {
            var types = await _companyContactService.GetTypeOfBusiness();
            return Ok(new { types });
        }

        [HttpGet("GetSICCode")]
        public async Task<IActionResult> GetSICCode(string typeOfBusiness)
        {
            var sicCode = await _companyContactService.GetSICCode(typeOfBusiness);

            if (sicCode == null)
            {
                return NotFound();
            }

            return Ok(new { SICCode = sicCode });
        }
    }
}
