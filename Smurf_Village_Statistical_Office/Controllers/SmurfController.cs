using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.Utils;
using System.Drawing;

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
        public async Task<IActionResult> GetSmurfs(
            [FromQuery] string? name, 
            [FromQuery] int? age,
            [FromQuery] string? job,
            [FromQuery] string? favouriteFood,
            [FromQuery] string? favouriteBrand,
            [FromQuery] string? favouriteColor)
        {
            var isNameInvalid = string.IsNullOrEmpty(name);
            var isAgeInvalid = age == null;
            var isJobInvalid = string.IsNullOrEmpty(job);
            var isFavouriteFoodInvalid = string.IsNullOrEmpty(favouriteFood);
            var isFavouriteBrandInvalid = string.IsNullOrEmpty(favouriteBrand);
            var isFavouriteColorInvalid = string.IsNullOrEmpty(favouriteColor);

            name = name?.ToLower();
            favouriteColor = favouriteColor?.ToLower();

            Enum.TryParse(job, true, out Job parsedJob);
            Enum.TryParse(favouriteFood, true, out Food parsedFavouriteFood);
            Enum.TryParse(favouriteBrand, true, out Brand parsedFavouriteBrand);

            var smurfs = await _context.Smurfs
                .Where(s => 
                    (isNameInvalid || (s.Name != null && s.Name.ToLower() == name)) &&
                    (isAgeInvalid || s.Age == age) &&
                    (isJobInvalid || s.Job == parsedJob) &&
                    (isFavouriteFoodInvalid || s.FavouriteFood == parsedFavouriteFood) &&
                    (isFavouriteBrandInvalid || s.FavouriteBrand == parsedFavouriteBrand) &&
                    (isFavouriteColorInvalid || s.FavouriteColor.Name.ToLower() == favouriteColor))
                .AsNoTracking()
                .ToListAsync();

            return Ok(smurfs);
        }

        [HttpGet("Smurfs/{id}")]
        public async Task<IActionResult> GetSmurf([FromRoute] int id)
        {
            var smurf = await _context.Smurfs
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return smurf == null ? NotFound() : Ok(smurf);  
        }
    }
}
