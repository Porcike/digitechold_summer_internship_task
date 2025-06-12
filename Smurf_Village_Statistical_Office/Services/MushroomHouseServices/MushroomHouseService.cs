using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Models;

namespace Smurf_Village_Statistical_Office.Services.MushroomHouseService
{
    public class MushroomHouseService : IMushroomHouseService
    {
        private readonly SmurfVillageContext _context;

        public MushroomHouseService(SmurfVillageContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MushroomHouseDto>> GetAllAsync(MushroomHouseFilterDto filter)
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
                .Select(h => ToDto(h))
                .AsNoTracking()
                .ToListAsync();

            return houses;
        }

        public async Task<MushroomHouseDto?> GetByIdAsnyc(int id)
        {
            var house = await _context.MushroomHouses
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);

            return house != null 
                ? ToDto(house) 
                : null;
        }

        private MushroomHouseDto ToDto(MushroomHouse house)
        {
            return new MushroomHouseDto
            {
                Id = house.Id,
                Capacity = house.Capacity,
                Color = new ColorDto
                {
                    Name = house.Color.Name,
                    Red = house.Color.R,
                    Green = house.Color.G,
                    Blue = house.Color.B,
                    Alpha = house.Color.A
                },
                Motto = house.Motto,
                ResidentIds = house.Residents.Select(r => r.Id).ToList()
            };
        }
    }
}
