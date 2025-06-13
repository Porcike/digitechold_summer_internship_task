using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;
using System.Data;

namespace Smurf_Village_Statistical_Office.Services.LeisureVenueServices.General
{
    public class LeisureVenueService(SmurfVillageContext context) : ILeisureVenueService
    {
        private readonly SmurfVillageContext _context = context;

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
            var (success, message) = await CheckForGeneralConstraints(value);
            if (!success)
            {
                throw new ArgumentException(message);
            }

            var members = await _context.Smurfs
                .Where(s => value.MemberIds.Contains(s.Id))
                .ToListAsync();

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

        public async Task UpdateAsync(UpdateLeisureVenueDto value)
        {
            var (success, message) = await CheckForGeneralConstraints(value);
            if (!success)
            {
                throw new ArgumentException(message);
            }

            var venue = await _context.LeisureVenues.FindAsync(value.Id);
            if (venue == null)
            {
                throw new KeyNotFoundException();
            }

            await _context.Entry(venue)
                .Collection(v => v.Members)
                .LoadAsync();

            var members = await _context.Smurfs
                .Where(s => value.MemberIds.Contains(s.Id))
                .ToListAsync();

            venue.Name = value.Name;
            venue.Capacity = value.Capacity;
            venue.Members = members;
            venue.AcceptedBrand = (Brand)value.AcceptedBrand;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var venue = await _context.LeisureVenues.FindAsync(id);
            if (venue == null)
            {
                throw new KeyNotFoundException();
            }

            var hasMembers = await _context.LeisureVenues
                .Where(l => l.Id == id)
                .SelectMany(l => l.Members)
                .AsNoTracking()
                .AnyAsync();

            if (hasMembers)
            {
                throw new InvalidOperationException("This venue still has members!");
            }
 
            _context.LeisureVenues.Remove(venue);
            await _context.SaveChangesAsync();
        }

        public async Task AddMemberAsync(int id, int memberId)
        {
            var venue = await _context.LeisureVenues.FindAsync(id);
            if (venue == null)
            {
                throw new KeyNotFoundException("Unknown venue!");
            }

            var member = await _context.Smurfs.FindAsync(memberId);
            if (member == null)
            {
                throw new KeyNotFoundException("Unknown smurf!");
            }

            if (venue.AcceptedBrand != member.FavouriteBrand)
            {
                throw new InvalidOperationException("The accepted brand isn't compatible with the smurf's favourite brand!");
            }

            if (venue.Members.Contains(member))
            {
                throw new InvalidOperationException("This smurf is already a member!");
            }

            var memberCount = _context.LeisureVenues
                .Where(l => l.Id == id)
                .Select(l => l.Members.Count)
                .First();

            if (memberCount + 1 > venue.Capacity)
            {
                throw new InvalidOperationException("The venue can't take any more members!");
            }

            venue.Members.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(int id, int memberId)
        {
            var venue = await _context.LeisureVenues.FindAsync(id);
            if (venue == null)
            {
                throw new KeyNotFoundException("Unknown venue!");
            }

            var member = await _context.Smurfs.FindAsync(memberId);
            if (member == null)
            {
                throw new KeyNotFoundException("Unknown smurf!");
            }

            if (!venue.Members.Contains(member))
            {
                throw new KeyNotFoundException("This smurf is not a member!");
            }

            venue.Members.Remove(member);
            await _context.SaveChangesAsync();
        }

        private async Task<(bool, string?)> CheckForGeneralConstraints(BaseLeisureVenueDto value)
        {
            if (value.Capacity < value.MemberIds.Count)
            {
                return (false, "The capacity is too small!");
            }

            if(!Enum.IsDefined(typeof(Brand), value.AcceptedBrand))
            {
                return (false, "Unknown brand!");
            }

            var memberCount = await _context.Smurfs
                .CountAsync(s => value.MemberIds.Contains(s.Id));

            if (value.MemberIds.Count != memberCount)
            {
                return (false, "Unknown member!");
            }

            var isBrandIncompatible = await _context.Smurfs
                .AnyAsync(s => 
                    value.MemberIds.Contains(s.Id) &&
                    (int)s.FavouriteBrand != value.AcceptedBrand);

            if (isBrandIncompatible)
            {
                return (false, "The accepted brand isn't compatible with the members' favourite brand!");
            }

            return (true, null);
        }
    }
}
