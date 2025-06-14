using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO.ColorDtos;
using Smurf_Village_Statistical_Office.DTO.SmurfDtos;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Services.General;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.SmurfServices.General
{
    public class SmurfService(SmurfVillageContext context) : ISmurfService
    {
        private readonly SmurfVillageContext _context = context;

        private string[] acceptedParams = ["Name", "Age"];

        public async Task<IReadOnlyCollection<SmurfDto>> GetAllAsync(SmurfFilterDto filter, int page, int pageSize, string? orderBy)
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

            var smurfsQuery = _context.Smurfs
                .AsNoTracking()
                .Where(s =>
                    (isNameProvided || EF.Functions.Like(s.Name, filter.name)) &&
                    (isMinAgeProvided || s.Age >= filter.minAge) &&
                    (isMaxAgeProvided || s.Age <= filter.maxAge) &&
                    (isJobProvided || jobParseWasSuccessFul && s.Job == parsedJob) &&
                    (isFavouriteFoodProvided || foodParseWasSuccessFul && s.FavouriteFood == parsedFavouriteFood) &&
                    (isFavouriteBrandProvided || brandParseWasSuccessful && s.FavouriteBrand == parsedFavouriteBrand) &&
                    (isFavouriteColorProvided || EF.Functions.Like(s.FavouriteColor.Name, filter.favouriteColor)));

            if (!smurfsQuery.Any())
            {
                return new List<SmurfDto>();
            }

            if(!string.IsNullOrWhiteSpace(orderBy))
            {
                smurfsQuery = IEntityService<
                    Smurf,
                    SmurfDto,
                    CreateSmurfDto,
                    UpdateSmurfDto,
                    SmurfFilterDto>
                    .Order(smurfsQuery, acceptedParams, orderBy);
            }

            pageSize = Math.Min(pageSize, 100);

            return await smurfsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SmurfDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    Job = s.Job,
                    FavouriteFood = s.FavouriteFood,
                    FavouriteBrand = s.FavouriteBrand,
                    FavouriteColor = ColorDto.FromColor(s.FavouriteColor)
                })
                .ToListAsync();
        }

        public async Task<SmurfDto?> GetByIdAsnyc(int id)
        {
            return await _context.Smurfs
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new SmurfDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    Job = s.Job,
                    FavouriteFood = s.FavouriteFood,
                    FavouriteBrand = s.FavouriteBrand,
                    FavouriteColor = ColorDto.FromColor(s.FavouriteColor)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<SmurfDto> InsertAsync(CreateSmurfDto value)
        {
            if(!CheckGeneralConstraints(value, out var message))
            {
                throw new ArgumentException(message);
            }

            var smurf = new Smurf
            {
                Name = value.Name,
                Age = value.Age,
                Job = (Job)value.Job,
                FavouriteFood = (Food)value.FavouriteFood,
                FavouriteBrand = (Brand)value.FavouriteBrand,
                FavouriteColor = ColorDto.ToColor(value.FavouriteColor)
            };

            await _context.Smurfs.AddAsync(smurf);
            await _context.SaveChangesAsync();

            return new SmurfDto
            {
                Id = smurf.Id,
                Name = smurf.Name,
                Age = smurf.Age,
                Job = smurf.Job,
                FavouriteFood = smurf.FavouriteFood,
                FavouriteBrand = smurf.FavouriteBrand,
                FavouriteColor = ColorDto.FromColor(smurf.FavouriteColor)
            };
        }

        public async Task UpdateAsync(UpdateSmurfDto value)
        {
            if(!CheckGeneralConstraints(value, out var message))
            {
                throw new ArgumentException(message);
            }

            var smurf = await _context.Smurfs.FindAsync(value.Id);
            if(smurf == null)
            {
                throw new KeyNotFoundException("Smurf not found!");
            }

            var isIncompatibleWithWorkplaces = await _context.WorkingPlaces
                .AnyAsync(w =>
                    w.Employees.Any(e => e.Id == value.Id) &&
                    !w.AcceptedJobs.Contains((Job)value.Job));

            if(isIncompatibleWithWorkplaces)
            {
                throw new ArgumentException("Incompatible job!");
            }

            var isIncompatibleWithHouses = await _context.MushroomHouses
                .AnyAsync(m =>
                    m.Residents.Any(r => r.Id == value.Id) &&
                    m.Color.ToArgb() == ColorDto.ToColor(value.FavouriteColor).ToArgb());

            if(isIncompatibleWithHouses)
            {
                throw new ArgumentException("Incompatible color!");
            }

            var isIncompatibleVenues = await _context.LeisureVenues
                .AnyAsync(v => 
                    v.Members.Any(m => m.Id == value.Id) &&
                    (int)v.AcceptedBrand != value.FavouriteBrand);

            if (isIncompatibleVenues)
            {
                throw new ArgumentException("Incompatible brand!");
            }

            smurf.Name = value.Name;
            smurf.Age = value.Age;
            smurf.Job = (Job)value.Job;
            smurf.FavouriteFood = (Food)value.FavouriteFood;
            smurf.FavouriteBrand = (Brand)value.FavouriteBrand;
            smurf.FavouriteColor = ColorDto.ToColor(value.FavouriteColor);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var smurf = await _context.Smurfs.FindAsync(id);
            if(smurf == null)
            {
                throw new KeyNotFoundException("Smurf not found!");
            }

            _context.Smurfs.Remove(smurf);
            await _context.SaveChangesAsync();
        }

        private bool CheckGeneralConstraints(BaseSmurfDto value, out string? message)
        {
            if (!Enum.IsDefined(typeof(Job), value.Job))
            {
                message = "Unknown job!";
                return false;
            }

            if (!Enum.IsDefined(typeof(Food), value.FavouriteFood))
            {
                message = "Unknown food!";
                return false;
            }

            if (!Enum.IsDefined(typeof(Brand), value.FavouriteBrand))
            {
                message = "Unknown brand!";
                return false;
            }

            message = null;
            return true;
        }
    }
}
