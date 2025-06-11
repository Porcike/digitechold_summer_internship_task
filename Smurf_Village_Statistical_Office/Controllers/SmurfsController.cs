using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Services;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class SmurfsController : ControllerBase
    {
        private readonly ISmurfService _smurfService;

        public SmurfsController(ISmurfService smurfService)
        {
            _smurfService = smurfService;
        }

        [HttpGet("Smurfs")]
        public async Task<IActionResult> GetSmurfs([FromQuery] SmurfFilterDto filter)
        {
            var smurfs = await _smurfService.GetAllAsync(filter);
            return Ok(smurfs);
        }

        [HttpGet("Smurfs/{id}")]
        public async Task<IActionResult> GetSmurf([FromRoute] int id)
        {
            var smurf = await _smurfService.GetByIdAsnyc(id);
            return smurf == null ? NotFound() : Ok(smurf);
        }
    }
}
