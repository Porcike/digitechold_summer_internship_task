using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class SmurfController : ControllerBase
    {
        private readonly SmurfVillageContext _context;

        public SmurfController(SmurfVillageContext context)
        {
            _context = context;
        }

        [HttpGet("Smurfs")]
        public async Task<IActionResult> GetSmurfs()
        {
            var smurfs = await _context.Smurfs
                .AsNoTracking()
                .ToListAsync();

            return Ok(smurfs);
        }
    }
}
