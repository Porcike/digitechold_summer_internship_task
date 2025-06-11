using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class WorkingPlacesController : ControllerBase
    {
        private readonly SmurfVillageContext _context;

        public WorkingPlacesController(SmurfVillageContext context)
        {
            _context = context;
        }

        [HttpGet("WorkingPlaces")]
        public async Task<IActionResult> GetWorkingPlaces(
            [FromQuery] string? name,
            [FromQuery] string? employee,
            [FromQuery] string? job)
        {
            var isNameInvalid = string.IsNullOrEmpty(name);
            var isEmployeeInvalid = string.IsNullOrEmpty(employee);
            var isJobInvalid = string.IsNullOrEmpty(job);

            var jobParseWasSuccessFul = Enum.TryParse(job, true, out Job parsedJob);

            var workplaces = await _context.WorkingPlaces
                .Where(w => 
                    (isNameInvalid || EF.Functions.Like(w.Name, name)) &&
                    (isEmployeeInvalid || w.Employees.Any(e => EF.Functions.Like(e.Name, employee))) &&
                    (isJobInvalid || (jobParseWasSuccessFul && w.AcceptedJobs.Contains(parsedJob))))
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    w.AcceptedJobs,
                    EmployeeIds = w.Employees.Select(e => e.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(workplaces);
        }

        [HttpGet("WorkingPlaces/{id}")]
        public async Task<IActionResult> GetWorkingPlace([FromRoute] int id)
        {
            var workingPlace = await _context.WorkingPlaces
                .Where(w => w.Id == id)
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    w.AcceptedJobs,
                    EmployeeIds = w.Employees.Select(e => e.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return workingPlace == null ? NotFound() : Ok(workingPlace);
        }
    }
}
