using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.SmurfService
{
    public class SmurfService : ISmurfService
    {
        private readonly SmurfVillageContext _context;

        public SmurfService(SmurfVillageContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SmurfDto>> GetAllAsync(SmurfFilterDto filter)
        {
            var isNameProvided = string.IsNullOrEmpty(filter.name);
            var isMinAgeProvided = filter.minAge == null;
            var isMaxAgeProvided = filter.maxAge == null;
            var isJobProvided = string.IsNullOrEmpty(filter.job);
            var isFavouriteFoodProvided = string.IsNullOrEmpty(filter.favouriteFood);
            var isFavouriteBrandProvided = string.IsNullOrEmpty(filter.favouriteBrand);
            var isFavouriteColorProvided = string.IsNullOrEmpty(filter.favouriteColor);

            var jobParseWasSuccessFul = Enum.TryParse(filter.job, true, out Job parsedJob);
            var foodParseWasSuccessFul = Enum.TryParse(filter.favouriteFood, true, out Food parsedFavouriteFood);
            var brandParseWasSuccessful = Enum.TryParse(filter.favouriteBrand, true, out Brand parsedFavouriteBrand);

            var smurfs = await _context.Smurfs
                .Where(s =>
                    (isNameProvided || EF.Functions.Like(s.Name, filter.name)) &&
                    (isMinAgeProvided || s.Age >= filter.minAge) &&
                    (isMaxAgeProvided || s.Age <= filter.maxAge) &&
                    (isJobProvided || jobParseWasSuccessFul && s.Job == parsedJob) &&
                    (isFavouriteFoodProvided || foodParseWasSuccessFul && s.FavouriteFood == parsedFavouriteFood) &&
                    (isFavouriteBrandProvided || brandParseWasSuccessful && s.FavouriteBrand == parsedFavouriteBrand) &&
                    (isFavouriteColorProvided || EF.Functions.Like(s.FavouriteColor.Name, filter.favouriteColor)))
                .Select(s => new SmurfDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    Job = s.Job,
                    FavouriteFood = s.FavouriteFood,
                    FavouriteBrand = s.FavouriteBrand,
                    FavouriteColor = new ColorDto
                    {
                        Name = s.FavouriteColor.Name,
                        Red = s.FavouriteColor.R,
                        Green = s.FavouriteColor.G,
                        Blue = s.FavouriteColor.B,
                        Alpha = s.FavouriteColor.A
                    }
                })
                .AsNoTracking()
                .ToListAsync();

            return smurfs;
        }

        public async Task<SmurfDto?> GetByIdAsnyc(int id)
        {
            var smurf = await _context.Smurfs
                .Where(s => s.Id == id)
                .Select(s => new SmurfDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    Job = s.Job,
                    FavouriteFood = s.FavouriteFood,
                    FavouriteBrand = s.FavouriteBrand,
                    FavouriteColor = new ColorDto
                    {
                        Name = s.FavouriteColor.Name,
                        Red = s.FavouriteColor.R,
                        Green = s.FavouriteColor.G,
                        Blue = s.FavouriteColor.B,
                        Alpha = s.FavouriteColor.A
                    }
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return smurf;
        }

    }
}
