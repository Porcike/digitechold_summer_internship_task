using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Services;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class MushroomHousesController : ControllerBase
    {
        private readonly IMushroomHouseService _mushroomService;

        public MushroomHousesController(IMushroomHouseService mushroomService)
        {
            _mushroomService = mushroomService;
        }

        [HttpGet("MushroomHouses")]
        public async Task<IActionResult> GetMushroomHouses([FromQuery] MushroomHouseFilterDto filter)
        {
            var houses = await _mushroomService.GetAllAsync(filter);
            return Ok(houses);
        }

        [HttpGet("MushroomHouses/{id}")]
        public async Task<IActionResult> GetMushroomHouse([FromRoute] int id)
        {
            var house = await _mushroomService.GetByIdAsnyc(id);
            return house == null ? NotFound() : Ok(house);
        }
    }
}
