using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly SmurfVillageContext _context;

        public BrandController(SmurfVillageContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Brands")]
        public async Task<IActionResult> GetBrands()
        {
            var items = Enum.GetValues<Brand>()
                 .Cast<Brand>()
                 .Select(b => new
                 {
                     Id = (int)b,
                     Name = b.ToString()
                 });
            return Ok(items);
        }
    }
}
