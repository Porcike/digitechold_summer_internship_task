using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;

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
                .Select(h => new MushroomHouseDto
                {
                    Id = h.Id,
                    Capacity = h.Capacity,
                    Color = new ColorDto
                    {
                        Name = h.Color.Name,
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
                        Name = h.Color.Name,
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
    }
}
