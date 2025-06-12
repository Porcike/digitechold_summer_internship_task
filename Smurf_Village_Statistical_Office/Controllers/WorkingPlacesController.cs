using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Services.WorkingPlaceService;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class WorkingPlacesController : ControllerBase
    {
        private readonly IWorkingPlaceService _workingplaceService;

        public WorkingPlacesController(IWorkingPlaceService workingPlaceService)
        {
            _workingplaceService = workingPlaceService;
        }

        [HttpGet("WorkingPlaces")]
        public async Task<IActionResult> GetWorkingPlaces([FromQuery] WorkingPlaceFilterDto filter)
        {
            var workingPlaces = await _workingplaceService.GetAllAsync(filter);
            return Ok(workingPlaces);
        }

        [HttpGet("WorkingPlaces/{id}")]
        public async Task<IActionResult> GetWorkingPlace([FromRoute] int id)
        {
            var workingPlace = await _workingplaceService.GetByIdAsnyc(id);
            return workingPlace == null ? NotFound() : Ok(workingPlace);
        }
    }
}
