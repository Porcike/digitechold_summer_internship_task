using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class LeisureVenueController : ControllerBase
    {
        private readonly SmurfVillageContext _context;

        public LeisureVenueController(SmurfVillageContext context)
        {
            _context = context;
        }

        [HttpGet("LeisureVenues")]
        public async Task<IActionResult> GetLeisureVenues()
        {
            var venues = await _context.LeisureVenues

                .Select(v => new
                {
                    v.Id,
                    v.Name,
                    v.Capacity,
                    v.AcceptedBrand,
                    MemberIds = v.Members.Select(m => m.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(venues);
        }
    }
}
