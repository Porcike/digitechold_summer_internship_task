using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly SmurfVillageContext _context;

        public JobsController(SmurfVillageContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Foods")]
        public async Task<IActionResult> GetFoods()
        {
            var items = Enum.GetValues<Food>()
                 .Cast<Food>()
                 .Select(f => new
                 {
                     Id = (int)f,
                     Name = f.ToString()
                 });
            return Ok(items);
        }
    }
}
