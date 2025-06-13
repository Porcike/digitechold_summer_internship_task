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
            catch (KeyNotFoundException)
            {
                return NotFound();
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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("LeisureVenues/{id}/members/{memberId}")]
        public async Task<IActionResult> AddMember([FromRoute] int id, [FromRoute] int memberId)
        {
            try
            {
                await _leisureVenueService.AddMemberAsync(id, memberId);
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

        [HttpDelete("LeisureVenues/{id}/members/{memberId}")]
        public async Task<IActionResult> RemoveMember([FromRoute] int id, [FromRoute] int memberId)
        {
            try
            {
                await _leisureVenueService.RemoveMemberAsync(id, memberId);
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
