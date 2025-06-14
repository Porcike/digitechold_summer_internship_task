using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO.BrandDtos;
using Smurf_Village_Statistical_Office.Utils;
using System.Diagnostics;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class BrandsController(SmurfVillageContext context) : ControllerBase
    {
        private readonly SmurfVillageContext _context = context;

        [HttpGet]
        [Route("Brands")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = Enum.GetValues<Brand>()
                 .Cast<Brand>()
                 .Select(b => new BrandDto
                 {
                     Id = (int)b,
                     Name = b.ToString()
                 });

            return Ok(brands);
        }

        [HttpGet]
        [Route("Brands/{id}")]
        public async Task<IActionResult> GetBrand([FromRoute] int id)
        {
            var isValid = Enum.IsDefined(typeof(Brand), id);
            return isValid
                ? Ok(new BrandDto
                {
                    Id = id,
                    Name = ((Brand)id).ToString()
                })
                : NotFound(new
                {
                    message = "Brand not found!"
                });
        }
    }
}
