using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos;
using Smurf_Village_Statistical_Office.Services.LeisureVenueServices.General;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class LeisureVenuesController(ILeisureVenueService leisureVenueService) : ControllerBase
    {
        private readonly ILeisureVenueService _leisureVenueService = leisureVenueService;

        [HttpGet("LeisureVenues")]
        public async Task<IActionResult> List(
            [FromQuery] LeisureVenueFilterDto filter,
            [FromQuery] string? orderBy,
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var venues = await _leisureVenueService.GetAllAsync(filter, page,  pageSize, orderBy);
            return Ok(venues);
        }

        [HttpGet("LeisureVenues/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var venue = await _leisureVenueService.GetByIdAsnyc(id);
            return venue != null 
                ? Ok(venue)
                : NotFound(new 
                {
                    message = "Venue not found!"
                });
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
                return BadRequest(new 
                {
                    message = ex.Message,
                });
            }
        }

        [HttpPut("LeisureVenues")]
        public async Task<IActionResult> Update([FromBody] UpdateLeisureVenueDto value)
        {
            try
            {
                await _leisureVenueService.UpdateAsync(value);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new 
                {
                    message = ex.Message,
                });
            }
        }

        [HttpDelete("LeisureVenues/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _leisureVenueService.DeleteAsync(id);
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

        [HttpPost("LeisureVenues/{venueId}/members/{smurfId}")]
        public async Task<IActionResult> AddMember([FromRoute] int venueId, [FromRoute] int smurfId)
        {
            try
            {
                await _leisureVenueService.AddMemberAsync(venueId, smurfId);
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

        [HttpDelete("LeisureVenues/{venueId}/members/{smurfId}")]
        public async Task<IActionResult> RemoveMember([FromRoute] int venueId, [FromRoute] int smurfId)
        {
            try
            {
                await _leisureVenueService.RemoveMemberAsync(venueId, smurfId);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
