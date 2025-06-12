using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly SmurfVillageContext _context;

        public FoodsController(SmurfVillageContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Foods")]
        public async Task<IActionResult> GetFoods()
        {
            var foods = Enum.GetValues<Food>()
                 .Cast<Food>()
                 .Select(f => new FoodDto
                 {
                     Id = (int)f,
                     Name = f.ToString()
                 });

            return Ok(foods);
        }

        [HttpGet]
        [Route("Foods/{id}")]
        public async Task<IActionResult> GetFood([FromRoute] int id)
        {
            var isValid = Enum.IsDefined(typeof(Food), id);
            return isValid 
                ? Ok(new FoodDto 
                {
                    Id = id,
                    Name = ((Food)id).ToString()
                }) 
                : NotFound();
        }
    }
}
