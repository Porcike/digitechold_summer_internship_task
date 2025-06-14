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
        public async Task<IActionResult> List(
            [FromQuery] WorkingPlaceFilterDto filter,
            [FromQuery] string? orderBy,
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var workingPlaces = await _workingplaceService.GetAllAsync(filter, page, pageSize, orderBy);
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
                return BadRequest(new 
                {
                    message = ex.Message
                });
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

        [HttpDelete("WorkingPlaces/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _workingplaceService.DeleteAsync(id);
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

        [HttpPost("WorkingPlaces/{workplaceId}/employees/{smurfId}")]
        public async Task<IActionResult> AddEmployee([FromRoute] int workplaceId, [FromRoute] int smurfId)
        {
            try
            {
                await _workingplaceService.AddEmployeeAsync(workplaceId, smurfId);
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

        [HttpDelete("WorkingPlaces/{workplaceId}/employees/{smurfId}")]
        public async Task<IActionResult> RemoveEmployee([FromRoute] int workplaceId, [FromRoute] int smurfId)
        {
            try
            {
                await _workingplaceService.RemoveEmployeeAsync(workplaceId, smurfId);
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
