using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Services.MushroomHouseService;

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
        public async Task<IActionResult> List([FromQuery] MushroomHouseFilterDto filter)
        {
            var houses = await _mushroomService.GetAllAsync(filter);
            return Ok(houses);
        }

        [HttpGet("MushroomHouses/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var house = await _mushroomService.GetByIdAsnyc(id);
            return house == null ? NotFound() : Ok(house);
        }

        [HttpPost("MushroomHouses")]
        public async Task<IActionResult> Create([FromBody] CreateMushroomHouseDto value)
        {
            try
            {
                var created = await _mushroomService.InsertAsync(value);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
