using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class LeisureVenuesController : ControllerBase
    {
        private readonly SmurfVillageContext _context;

        public LeisureVenuesController(SmurfVillageContext context)
        {
            _context = context;
        }

        [HttpGet("LeisureVenues")]
        public async Task<IActionResult> GetLeisureVenues(
            [FromQuery] string? name,
            [FromQuery] int? minCapacity,
            [FromQuery] int? maxCapacity,
            [FromQuery] string? member,
            [FromQuery] string? brand)
        {
            var isNameInvalid = string.IsNullOrEmpty(name);
            var isMinCapacityInvalid = minCapacity == null;
            var isMaxCapacityInvalid = maxCapacity == null;
            var isMemberInvalid = string.IsNullOrEmpty(member);
            var isBrandInvalid = string.IsNullOrEmpty(brand);

            var brandParseWasSuccessful = Enum.TryParse(brand, true, out Brand parsedBrand);

            var venues = await _context.LeisureVenues
                .Where(v => 
                    (isNameInvalid || EF.Functions.Like(v.Name, name)) &&
                    (isMinCapacityInvalid || v.Capacity >= minCapacity) &&
                    (isMaxCapacityInvalid || v.Capacity <= maxCapacity) &&
                    (isMemberInvalid || v.Members.Any(m => EF.Functions.Like(m.Name, member))) &&
                    (isBrandInvalid || (brandParseWasSuccessful && v.AcceptedBrand == parsedBrand)))
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

        [HttpGet("LeisureVenues/{id}")]
        public async Task<IActionResult> GetLeisureVenue([FromRoute] int id)
        {
            var venue = await _context.LeisureVenues.
                Where(v => v.Id == id)
                .Select(v => new
                {
                    v.Id,
                    v.Name,
                    v.Capacity,
                    v.AcceptedBrand,
                    MemberIds = v.Members.Select(m => m.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return venue == null ? NotFound() : Ok(venue);
        }
    }
}
