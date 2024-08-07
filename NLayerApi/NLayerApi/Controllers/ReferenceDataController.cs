using DataAccess.Entities;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Interfaces;
using Common.Dto;

namespace NLayerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //public class ReferenceDataController() : ControllerBase
    //{
    public class ReferenceDataController : Controller
    {
        private readonly DataContext _context;

        public ReferenceDataController(DataContext context)
        {
            _context = context;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReferenceData>>> GetReferenceDatas()
        {
            var referenceDatas = await _context.ReferenceDatas.ToListAsync();
            return Ok(referenceDatas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReferenceData>> GetReferenceData(int id)
        {
            var referenceData = await _context.ReferenceDatas.FindAsync(id);

            if (referenceData == null)
            {
                return NotFound();
            }

            return Ok(referenceData);
        }

        [HttpGet("organisation/{organisationId}")]
        public async Task<ActionResult<IEnumerable<ReferenceDataDto>>> GetReferenceDataByOrganisation(int organisationId)
        {
            var organisation = await _context.Organisations
                .Include(o => o.OrganisationReferenceDatas)
                .ThenInclude(ord => ord.ReferenceData)
                .FirstOrDefaultAsync(o => o.OrganisationId == organisationId);

            if (organisation == null)
            {
                return NotFound(new { Message = "Organisation not found" });
            }

            var referenceDataList = organisation.OrganisationReferenceDatas
                .Select(ord => new ReferenceDataDto
                {
                    RefId = ord.ReferenceData.RefId,
                    RefCode = ord.ReferenceData.RefCode,
                    RefValue = ord.ReferenceData.RefValue
                })
                .ToList();

            return Ok(referenceDataList);
        }
    }
}
