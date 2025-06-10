using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class WorkingPlaceController : ControllerBase
    {
        private readonly SmurfVillageContext _context;

        public WorkingPlaceController(SmurfVillageContext context)
        {
            _context = context;
        }

        [HttpGet("WorkingPlaces")]
        public async Task<IActionResult> GetWorkingPlaces()
        {
            var workplaces = await _context.WorkingPlaces
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    AcceptedJobs = w.AcceptedJobs,
                    EmployeeIds = w.Employees.Select(e => e.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(workplaces);
        }
    }
}
