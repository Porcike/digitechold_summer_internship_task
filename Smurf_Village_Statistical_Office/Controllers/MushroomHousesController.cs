using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.MushroomHouseDtos;
using Smurf_Village_Statistical_Office.Services.MushroomHouseServices.General;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class MushroomHousesController(IMushroomHouseService mushroomService) : ControllerBase
    {
        private readonly IMushroomHouseService _mushroomService = mushroomService;

        [HttpGet("MushroomHouses")]
        public async Task<IActionResult> List(
            [FromQuery] MushroomHouseFilterDto filter,
            [FromQuery] string? orderBy,
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var houses = await _mushroomService.GetAllAsync(filter, page, pageSize, orderBy);
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
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("MushroomHouses")]
        public async Task<IActionResult> Update([FromBody] UpdateMushroomHouseDto value)
        {
            try
            {
                await _mushroomService.UpdateAsync(value);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("MushroomHouses/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _mushroomService.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("MushroomHouses/{houseId}/residents/{smurfId}")]
        public async Task<IActionResult> AddResident([FromRoute] int houseId, [FromRoute] int smurfId)
        {
            try
            {
                await _mushroomService.AddResidentAsync(houseId, smurfId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("MushroomHouses/{houseId}/residents/{smurfId}")]
        public async Task<IActionResult> RemoveResident([FromRoute] int houseId, [FromRoute] int smurfId)
        {
            try
            {
                await _mushroomService.AddResidentAsync(houseId, smurfId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
