using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Services.LeisureVenueService;

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
        public async Task<IActionResult> GetLeisureVenues([FromQuery] LeisureVenueFilterDto filter)
        {
            var venues = await _leisureVenueService.GetAllAsync(filter);
            return Ok(venues);
        }

        [HttpGet("LeisureVenues/{id}")]
        public async Task<IActionResult> GetLeisureVenue([FromRoute] int id)
        {
            var venue = await _leisureVenueService.GetByIdAsnyc(id);
            return venue == null ? NotFound() : Ok(venue);
        }
    }
}
