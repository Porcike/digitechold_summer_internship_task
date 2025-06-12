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

        public async Task<IReadOnlyCollection<LeisureVenueDto>> GetAllAsync(LeisureVenueFilterDto filter)
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

        public async Task<LeisureVenueDto> InsertAsync(CreateLeisureVenueDto value)
        {
            if (value.Capacity < value.MemberIds.Count)
            {
                throw new ArgumentException("The capacity is too small!");
            }

            if (!Enum.IsDefined(typeof(Brand), value.AcceptedBrand))
            {
                throw new ArgumentException("Unknown brand!");
            }

            var members = await _context.Smurfs
                .Where(s => value.MemberIds.Contains(s.Id))
                .ToListAsync();

            if(value.MemberIds.Count != members.Count)
            {
                throw new ArgumentException("Unknown member!");
            }

            foreach(var member in members)
            {
                if((int)member.FavouriteBrand != value.AcceptedBrand)
                {
                    throw new ArgumentException("The venue's accepted brand isn't compatible with the members' favourite brand!");
                }
            }

            var venue = new LeisureVenue
            {
                Name = value.Name,
                Capacity = value.Capacity,
                Members = members,
                AcceptedBrand = (Brand)value.AcceptedBrand
            };

            await _context.LeisureVenues.AddAsync(venue);
            await _context.SaveChangesAsync();

            return new LeisureVenueDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Capacity = venue.Capacity,
                AcceptedBrand = venue.AcceptedBrand,
                MemberIds = venue.Members.Select(m => m.Id).ToList(),
            };
        }
    }
}
