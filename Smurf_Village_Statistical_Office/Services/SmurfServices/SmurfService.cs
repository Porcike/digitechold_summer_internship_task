using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Models;
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
                .Select(s => ToDto(s))
                .AsNoTracking()
                .ToListAsync();

            return smurfs;
        }

        public async Task<SmurfDto?> GetByIdAsnyc(int id)
        {
            var smurf = await _context.Smurfs
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return smurf != null 
                ? ToDto(smurf) 
                : null;
        }

        private SmurfDto ToDto(Smurf smurf)
        {
            return new SmurfDto
            {
                Id = smurf.Id,
                Name = smurf.Name,
                Age = smurf.Age,
                Job = smurf.Job,
                FavouriteFood = smurf.FavouriteFood,
                FavouriteBrand = smurf.FavouriteBrand,
                FavouriteColor = new ColorDto
                {
                    Name = smurf.FavouriteColor.Name,
                    Red = smurf.FavouriteColor.R,
                    Green = smurf.FavouriteColor.G,
                    Blue = smurf.FavouriteColor.B,
                    Alpha = smurf.FavouriteColor.A
                }
            };
        }
    }
}
