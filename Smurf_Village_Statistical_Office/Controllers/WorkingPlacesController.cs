using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.WorkingPlaceDtos;
using Smurf_Village_Statistical_Office.Services.WorkingPlaceServices.General;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class WorkingPlacesController(IWorkingPlaceService workingPlaceService) : ControllerBase
    {
        private readonly IWorkingPlaceService _workingplaceService = workingPlaceService;

        [HttpGet("WorkingPlaces")]
        public async Task<IActionResult> List([FromQuery] WorkingPlaceFilterDto filter)
        {
            var workingPlaces = await _workingplaceService.GetAllAsync(filter);
            return Ok(workingPlaces);
        }

        [HttpGet("WorkingPlaces/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var workingPlace = await _workingplaceService.GetByIdAsnyc(id);
            return workingPlace == null ? NotFound() : Ok(workingPlace);
        }


        [HttpPost("WorkingPlaces")]
        public async Task<IActionResult> Create([FromBody] CreateWorkingPlaceDto value)
        {
            try
            {
                var created = await _workingplaceService.InsertAsync(value);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("WorkingPlaces")]
        public async Task<IActionResult> Update([FromBody] UpdateWorkingPlaceDto value)
        {
            try
            {
                await _workingplaceService.UpdateAsync(value);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("WorkingPlaces/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _workingplaceService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
