using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO.JobDtos;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class JobsController(SmurfVillageContext context) : ControllerBase
    {
        private readonly SmurfVillageContext _context = context;

        [HttpGet]
        [Route("Jobs")]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = Enum.GetValues<Job>()
                 .Cast<Job>()
                 .Select(j => new JobDto
                 {
                     Id = (int)j,
                     Name = j.ToString()
                 });

            return Ok(jobs);
        }

        [HttpGet]
        [Route("Jobs/{id}")]
        public async Task<IActionResult> GetJob([FromRoute] int id)
        {
            var isValid = Enum.IsDefined(typeof(Job), id);
            return isValid
                ? Ok(new JobDto
                {
                    Id = id,
                    Name = ((Job)id).ToString()
                })
                : NotFound(new
                {
                    message = "Job not found!"
                });
        }
    }
}
