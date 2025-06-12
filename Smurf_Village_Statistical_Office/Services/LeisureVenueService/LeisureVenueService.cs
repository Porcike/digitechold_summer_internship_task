using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.LeisureVenueService
{
    public class LeisureVenueService : ILeisureVenueService
    {
        private readonly SmurfVillageContext _context;

        public LeisureVenueService(SmurfVillageContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeisureVenueDto>> GetAllAsync(LeisureVenueFilterDto filter)
        {
            var isNameProvided = string.IsNullOrEmpty(filter.name);
            var isMinCapacityProvided = filter.minCapacity == null;
            var isMaxCapacityProvided = filter.maxCapacity == null;
            var isMemberProvided = string.IsNullOrEmpty(filter.member);
            var isBrandProvided = string.IsNullOrEmpty(filter.brand);

            var brandParseWasSuccessful = Enum.TryParse(filter.brand, true, out Brand parsedBrand);

            var venues = await _context.LeisureVenues
                .Where(v =>
                    (isNameProvided || EF.Functions.Like(v.Name, filter.name)) &&
                    (isMinCapacityProvided || v.Capacity >= filter.minCapacity) &&
                    (isMaxCapacityProvided || v.Capacity <= filter.maxCapacity) &&
                    (isMemberProvided || v.Members.Any(m => EF.Functions.Like(m.Name, filter.member))) &&
                    (isBrandProvided || brandParseWasSuccessful && v.AcceptedBrand == parsedBrand))
                .Select(v => new LeisureVenueDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Capacity = v.Capacity,
                    AcceptedBrand = v.AcceptedBrand,
                    MemberIds = v.Members.Select(m => m.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return venues;
        }

        public async Task<LeisureVenueDto?> GetByIdAsnyc(int id)
        {
            var venue = await _context.LeisureVenues
                .Where(v => v.Id == id)
                .Select(v => new LeisureVenueDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Capacity = v.Capacity,
                    AcceptedBrand = v.AcceptedBrand,
                    MemberIds = v.Members.Select(m => m.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return venue;
        }
    }
}
