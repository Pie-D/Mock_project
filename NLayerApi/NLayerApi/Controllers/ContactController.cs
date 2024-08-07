using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace NLayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("listLeadContact")]
        public async Task<IActionResult> GetListLeadContact()
        {
            var listLeadContact = await _contactService.GetAllLeadContacts();
            //.Distinct() : tại mỗi khu vực có mã riêng mà, cần gì

            return Ok(new { listLeadContact });
        }
    }
}
