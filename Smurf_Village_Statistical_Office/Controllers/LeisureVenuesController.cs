using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos;
using Smurf_Village_Statistical_Office.DTO.WorkingPlaceDtos;
using Smurf_Village_Statistical_Office.Services.LeisureVenueServices.General;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class LeisureVenuesController : ControllerBase
    {
        private readonly ILeisureVenueService _leisureVenueService;

        public LeisureVenuesController(ILeisureVenueService leisureVenueService)
        {
            _leisureVenueService = leisureVenueService;
        }

        [HttpGet("LeisureVenues")]
        public async Task<IActionResult> List([FromQuery] LeisureVenueFilterDto filter)
        {
            var venues = await _leisureVenueService.GetAllAsync(filter);
            return Ok(venues);
        }

        [HttpGet("LeisureVenues/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var venue = await _leisureVenueService.GetByIdAsnyc(id);
            return venue == null ? NotFound() : Ok(venue);
        }

        [HttpPost("LeisureVenues")]
        public async Task<IActionResult> Create([FromBody] CreateLeisureVenueDto value)
        {
            try
            {
                var created = await _leisureVenueService.InsertAsync(value);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("LeisureVenues")]
        public async Task<IActionResult> Update([FromBody] UpdateLeisureVenueDto value)
        {
            try
            {
                await _leisureVenueService.UpdateAsync(value);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
