using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class MushroomHouseController : ControllerBase
    {
        private readonly SmurfVillageContext _context;

        public MushroomHouseController(SmurfVillageContext context)
        {
            _context = context;
        }

        [HttpGet("MushroomHouses")]
        public async Task<IActionResult> GetMushroomHouses()
        {
            var houses = await _context.MushroomHouses
                .Select(h => new
                {
                    h.Id,
                    h.Capacity,
                    h.Color,
                    h.Motto,
                    ResidentIds = h.Residents.Select(r => r.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(houses);
        }
    }
}
