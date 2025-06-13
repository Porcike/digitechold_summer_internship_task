using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO.MushroomHouseDtos;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.MushroomHouseServices.General
{
    public class MushroomHouseService(SmurfVillageContext context) : IMushroomHouseService
    {
        private readonly SmurfVillageContext _context = context;

        public async Task<IReadOnlyCollection<MushroomHouseDto>> GetAllAsync(MushroomHouseFilterDto filter)
        {
            var isResidentProvided = string.IsNullOrEmpty(filter.resident);
            var isMinCapacityProvided = filter.minCapacity == null;
            var isMaxCapacityProvided = filter.maxCapacity == null;
            var isColorProvided = string.IsNullOrEmpty(filter.color);

            var houses = await _context.MushroomHouses
                .Where(h =>
                    (isResidentProvided || h.Residents.Any(s => EF.Functions.Like(s.Name, filter.resident))) &&
                    (isMinCapacityProvided || h.Capacity >= filter.minCapacity) &&
                    (isMaxCapacityProvided || h.Capacity <= filter.maxCapacity) &&
                    (isColorProvided || EF.Functions.Like(h.Color.Name, filter.color)))
                .Select(h => new MushroomHouseDto
                {
                    Id = h.Id,
                    Capacity = h.Capacity,
                    Color = h.Color,
                    Motto = h.Motto,
                    ResidentIds = h.Residents.Select(r => r.Id).ToList(),
                    AcceptedFoods = h.AcceptedFoods
                })
                .AsNoTracking()
                .ToListAsync();

            return houses;
        }

        public async Task<MushroomHouseDto?> GetByIdAsnyc(int id)
        {
            var house = await _context.MushroomHouses
                .Where(h => h.Id == id)
                .Select(h => new MushroomHouseDto
                {
                    Id = h.Id,
                    Capacity = h.Capacity,
                    Color = h.Color,
                    Motto = h.Motto,
                    ResidentIds = h.Residents.Select(r => r.Id).ToList(),
                    AcceptedFoods = h.AcceptedFoods
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return house;
        }

        public async Task<MushroomHouseDto> InsertAsync(CreateMushroomHouseDto value)
        {
            var (success, message) = await CheckForGeneralConstraints(value);
            if (!success)
            {
                throw new ArgumentException(message);
            }

            var residents = await _context.Smurfs
                .Where(s => value.ResidentIds.Contains(s.Id))
                .ToListAsync();

            var house = new MushroomHouse
            {
                Residents = residents,
                Capacity = value.Capacity,
                AcceptedFoods = value.AcceptedFoods.Select(f => (Food)f).ToList(),
                Color = value.Color,
                Motto = value.Motto,
            };

            await _context.MushroomHouses.AddAsync(house);
            await _context.SaveChangesAsync();

            return new MushroomHouseDto
            {
                Id = house.Id,
                Capacity = house.Capacity,
                Color = house.Color,
                Motto = house.Motto,
                ResidentIds = house.Residents.Select(r => r.Id).ToList(),
                AcceptedFoods = house.AcceptedFoods
            };
        }

        public async Task UpdateAsync(UpdateMushroomHouseDto value)
        {
            var (success, message) = await CheckForGeneralConstraints(value);
            if (!success)
            {
                throw new ArgumentException(message);
            }

            var house = await _context.MushroomHouses.FirstAsync(h => h.Id == value.Id);

            await _context.Entry(house)
                .Collection(h => h.Residents)
                .LoadAsync();

            var residents = await _context.Smurfs
                .Where(s => value.ResidentIds.Contains(s.Id))
                .ToListAsync();

            house.Residents = residents;
            house.Capacity = value.Capacity;
            house.AcceptedFoods = value.AcceptedFoods.Select(f => (Food)f).ToList();
            house.Color = value.Color;
            house.Motto = value.Motto;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var houseExists = await _context.MushroomHouses.AnyAsync(h => h.Id == id);
            if (!houseExists)
            {
                throw new KeyNotFoundException();
            }

            var hasResidents = await _context.MushroomHouses
                .Where(h => h.Id == id)
                .SelectMany(h => h.Residents)
                .AnyAsync();

            if (hasResidents)
            {
                throw new ArgumentException("This house has residents in it!");
            }

            var house = await _context.MushroomHouses.FirstAsync(h => h.Id == id);
            _context.MushroomHouses.Remove(house);
            await _context.SaveChangesAsync();
        }

        private async Task<(bool, string?)> CheckForGeneralConstraints(BaseMushroomHouseDto value)
        {
            if(value.Capacity < value.ResidentIds.Count)
            {
                return (false, "The capacity is too small!");
            }

            var residentCount = await _context.Smurfs
                .CountAsync(s => value.ResidentIds.Contains(s.Id));

            if (value.ResidentIds.Count != residentCount)
            {
                return (false, "Unknown resident!");
            }

            var isHouseColorIncompatible = await _context.Smurfs
                .AnyAsync(s =>
                    value.ResidentIds.Contains(s.Id) &&
                    s.FavouriteColor.ToArgb() == value.Color.ToArgb());

            if (isHouseColorIncompatible)
            {
                return (false, "The house color isn't compatible with the residents' favourite color!");
            }

            return (true, null);
        }
    }
}
