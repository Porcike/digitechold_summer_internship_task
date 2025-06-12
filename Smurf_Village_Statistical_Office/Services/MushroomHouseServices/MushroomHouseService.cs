using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;
using System.Drawing;

namespace Smurf_Village_Statistical_Office.Services.MushroomHouseService
{
    public class MushroomHouseService : IMushroomHouseService
    {
        private readonly SmurfVillageContext _context;

        public MushroomHouseService(SmurfVillageContext context)
        {
            _context = context;
        }

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
                    Color = new ColorDto
                    {
                        Red = h.Color.R,
                        Green = h.Color.G,
                        Blue = h.Color.B,
                        Alpha = h.Color.A
                    },
                    Motto = h.Motto,
                    ResidentIds = h.Residents.Select(r => r.Id).ToList()
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
                    Color = new ColorDto
                    {
                        Red = h.Color.R,
                        Green = h.Color.G,
                        Blue = h.Color.B,
                        Alpha = h.Color.A
                    },
                    Motto = h.Motto,
                    ResidentIds = h.Residents.Select(r => r.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return house;
        }

        public async Task<MushroomHouseDto> InsertAsync(CreateMushroomHouseDto value)
        {
            if(value.Capacity < value.ResidentIds.Count)
            {
                throw new ArgumentException("The capacity is too small!");
            }

            var residents = await _context.Smurfs
                .Where(s => value.ResidentIds.Contains(s.Id))
                .ToListAsync();

            if(value.ResidentIds.Count != residents.Count)
            {
                throw new ArgumentException("Unknown resident!");
            }

            var houseColor = Color.FromArgb(
                value.Color.Alpha,
                value.Color.Red,
                value.Color.Green,
                value.Color.Blue);

            foreach(var resident in residents)
            {
                if (resident.FavouriteColor.ToArgb() == houseColor.ToArgb())
                {
                    throw new ArgumentException("The house color isn't compatible with the residents' favourite color!");
                }
            }

            var house = new MushroomHouse
            {
                Residents = residents,
                Capacity = value.Capacity,
                AcceptedFoods = value.AcceptedFoods.Select(f => (Food)f).ToList(),
                Color = houseColor,
                Motto = value.Motto,
            };

            await _context.MushroomHouses.AddAsync(house);
            await _context.SaveChangesAsync();

            return new MushroomHouseDto
            {
                Id = house.Id,
                Capacity = house.Capacity,
                Color = new ColorDto 
                {
                    Red = house.Color.R,
                    Green = house.Color.G,
                    Blue = house.Color.B,
                    Alpha = house.Color.A
                },
                Motto = house.Motto,
                ResidentIds = house.Residents.Select(r => r.Id).ToList(),
            };
        }
    }
}
