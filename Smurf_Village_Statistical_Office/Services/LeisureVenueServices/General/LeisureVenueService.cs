﻿using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;
using System.Data;
using System.Linq.Expressions;

namespace Smurf_Village_Statistical_Office.Services.LeisureVenueServices.General
{
    public class LeisureVenueService(SmurfVillageContext context) : ILeisureVenueService
    {
        private readonly SmurfVillageContext _context = context;

        private readonly string[] _acceptedParams = ["Name", "Capacity"];

        public async Task<IReadOnlyCollection<LeisureVenueDto>> GetAllAsync(
            LeisureVenueFilterDto filter, 
            int page, 
            int pageSize,
            string? orderBy)
        {
            var isNameProvided = string.IsNullOrWhiteSpace(filter.name);
            var isMinCapacityProvided = filter.minCapacity == null;
            var isMaxCapacityProvided = filter.maxCapacity == null;
            var isMemberProvided = string.IsNullOrWhiteSpace(filter.member);
            var isBrandProvided = string.IsNullOrWhiteSpace(filter.brand);

            var brandParseWasSuccessful = Enum.TryParse(filter.brand, true, out Brand parsedBrand);

            var venuesQuery = _context.LeisureVenues
                .AsNoTracking()
                .Where(v =>
                    (isNameProvided || EF.Functions.Like(v.Name, filter.name)) &&
                    (isMinCapacityProvided || v.Capacity >= filter.minCapacity) &&
                    (isMaxCapacityProvided || v.Capacity <= filter.maxCapacity) &&
                    (isMemberProvided || v.Members.Any(m => EF.Functions.Like(m.Name, filter.member))) &&
                    (isBrandProvided || (brandParseWasSuccessful && v.AcceptedBrand == parsedBrand)));

            if (!venuesQuery.Any())
            {
                return new List<LeisureVenueDto>();
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                venuesQuery = OrderUtil<LeisureVenue>.Order(venuesQuery, _acceptedParams, orderBy);
            }

            pageSize = Math.Min(pageSize, 100);

            return await venuesQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(ToDtoExpression)
                    .ToListAsync();
        }

        public async Task<LeisureVenueDto?> GetByIdAsnyc(int id)
        {
            return await _context.LeisureVenues
                .AsNoTracking()
                .Where(v => v.Id == id)
                .Select(ToDtoExpression)
                .FirstOrDefaultAsync();
        }

        public async Task<LeisureVenueDto> InsertAsync(CreateLeisureVenueDto value)
        {
            var message = await CheckForGeneralConstraints(value);
            if (message != null)
            {
                throw new ArgumentException(message);
            }

            var members = await _context.Smurfs
                .Where(s => value.MemberIds.Contains(s.Id))
                .ToListAsync();

            var venue = ToModel(value, members);

            await _context.LeisureVenues.AddAsync(venue);
            await _context.SaveChangesAsync();

            return ToDto(venue);
        }

        public async Task UpdateAsync(UpdateLeisureVenueDto value)
        {
            var message = await CheckForGeneralConstraints(value);
            if (message != null)
            {
                throw new ArgumentException(message);
            }

            var venue = await _context.LeisureVenues.FindAsync(value.Id);
            if (venue == null)
            {
                throw new KeyNotFoundException("Venue not found!");
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
                throw new KeyNotFoundException("Venue not found!");
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

        public async Task AddMemberAsync(int venueId, int smurfId)
        {
            var venue = await _context.LeisureVenues.FindAsync(venueId);
            if (venue == null)
            {
                throw new KeyNotFoundException("Venue not found!");
            }

            var smurf = await _context.Smurfs.FindAsync(smurfId);
            if (smurf == null)
            {
                throw new KeyNotFoundException("Smurf not found!");
            }

            if (venue.AcceptedBrand != smurf.FavouriteBrand)
            {
                throw new InvalidOperationException("Incompatible brands!");
            }

            if (venue.Members.Contains(smurf))
            {
                throw new InvalidOperationException("This smurf is already a member!");
            }

            var memberCount = await _context.LeisureVenues
                .Where(l => l.Id == venueId)
                .Select(l => l.Members.Count)
                .FirstAsync();

            if (memberCount + 1 > venue.Capacity)
            {
                throw new InvalidOperationException("This venue can't take any more members!");
            }

            venue.Members.Add(smurf);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(int venueId, int smurfId)
        {
            var venue = await _context.LeisureVenues.FindAsync(venueId);
            if (venue == null)
            {
                throw new KeyNotFoundException("Venue not found!");
            }

            var smurf = await _context.Smurfs.FindAsync(smurfId);
            if (smurf == null)
            {
                throw new KeyNotFoundException("Smurf not found!");
            }

            if (!venue.Members.Contains(smurf))
            {
                throw new KeyNotFoundException("This smurf is not a member!");
            }

            venue.Members.Remove(smurf);
            await _context.SaveChangesAsync();
        }

        private async Task<string?> CheckForGeneralConstraints(BaseLeisureVenueDto value)
        {
            if (value.Capacity < value.MemberIds.Count)
            {
                return "The capacity is too small!";
            }

            if(!Enum.IsDefined(typeof(Brand), value.AcceptedBrand))
            {
                return "Unknown brand!";
            }

            var memberCount = await _context.Smurfs
                .CountAsync(s => value.MemberIds.Contains(s.Id));

            if (value.MemberIds.Count != memberCount)
            {
                return "Unknown smurf!";
            }

            var isBrandIncompatible = await _context.Smurfs
                .AnyAsync(s => 
                    value.MemberIds.Contains(s.Id) &&
                    (int)s.FavouriteBrand != value.AcceptedBrand);

            if (isBrandIncompatible)
            {
                return "Incompatible brands!";
            }

            return null;
        }

        private Expression<Func<LeisureVenue, LeisureVenueDto>> ToDtoExpression = v => new LeisureVenueDto
        {
            Id = v.Id,
            Name = v.Name,
            Capacity = v.Capacity,
            AcceptedBrand = v.AcceptedBrand,
            MemberIds = v.Members.Select(m => m.Id).ToList()
        };

        private LeisureVenueDto ToDto(LeisureVenue v) => new LeisureVenueDto
        {
            Id = v.Id,
            Name = v.Name,
            Capacity = v.Capacity,
            AcceptedBrand = v.AcceptedBrand,
            MemberIds = v.Members.Select(m => m.Id).ToList()
        };

        private LeisureVenue ToModel(BaseLeisureVenueDto value, List<Smurf> members) => new LeisureVenue
        {
            Name = value.Name,
            Capacity = value.Capacity,
            Members = members,
            AcceptedBrand = (Brand)value.AcceptedBrand
        };
    }
}
