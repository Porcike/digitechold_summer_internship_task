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
        public async Task<IActionResult> GetMushroomHouses(
            [FromQuery] string? resident,
            [FromQuery] int? minCapacity,
            [FromQuery] int? maxCapacity,
            [FromQuery] string? color)
        {
            var isResidentInvalid = string.IsNullOrEmpty(resident);
            var isMinCapacityInvalid = minCapacity == null;
            var isMaxCapacityInvalid = maxCapacity == null;
            var isColorInvalid = string.IsNullOrEmpty(color);

            resident = resident?.ToLower();
            color = color?.ToLower();

            var houses = await _context.MushroomHouses
                .Where(h =>
                    (isResidentInvalid || h.Residents.Any(s => s.Name != null && s.Name.ToLower() == resident)) &&
                    (isMinCapacityInvalid || h.Capacity >= minCapacity) && 
                    (isMaxCapacityInvalid || h.Capacity <= maxCapacity) &&
                    (isColorInvalid || h.Color.Name.ToLower() == color))
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

        [HttpGet("MushroomHouses/{id}")]
        public async Task<IActionResult> GetMushroomHouse([FromRoute] int id)
        {
            var house = await _context.MushroomHouses
                .Select(h => new
                {
                    h.Id,
                    h.Capacity,
                    h.Color,
                    h.Motto,
                    ResidentIds = h.Residents.Select(r => r.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);    

            return house == null ? NotFound() : Ok(house);
        }
    }
}
