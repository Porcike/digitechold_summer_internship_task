using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;

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

            var venue = await _context.LeisureVenues.FirstAsync(v => v.Id == value.Id);

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
            var venueExists = await _context.LeisureVenues.AnyAsync(v => v.Id == id);
            if (!venueExists)
            {
                throw new KeyNotFoundException();
            }

            var hasMembers = await _context.LeisureVenues
                .Where(l => l.Id == id)
                .SelectMany(l => l.Members)
                .AnyAsync();

            if (hasMembers)
            {
                throw new ArgumentException("This venue still has members!");
            }

            var venue = await _context.LeisureVenues.FirstAsync(v => v.Id == id);
            _context.LeisureVenues.Remove(venue);
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
                return (false, "The venue's accepted brand isn't compatible with the members' favourite brand!");
            }

            return (true, null);
        }
    }
}
