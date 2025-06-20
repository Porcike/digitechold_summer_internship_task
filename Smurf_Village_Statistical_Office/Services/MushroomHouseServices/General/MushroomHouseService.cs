﻿using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO.ColorDtos;
using Smurf_Village_Statistical_Office.DTO.MushroomHouseDtos;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;
using System.Linq.Expressions;

namespace Smurf_Village_Statistical_Office.Services.MushroomHouseServices.General
{
    public class MushroomHouseService(SmurfVillageContext context) : IMushroomHouseService
    {
        private readonly SmurfVillageContext _context = context;

        private readonly string[] _acceptedParams = ["Capacity"];

        public async Task<IReadOnlyCollection<MushroomHouseDto>> GetAllAsync(
            MushroomHouseFilterDto filter, 
            int page, 
            int pageSize, 
            string? orderBy)
        {
            var isResidentProvided = string.IsNullOrEmpty(filter.resident);
            var isMinCapacityProvided = filter.minCapacity == null;
            var isMaxCapacityProvided = filter.maxCapacity == null;
            var isColorProvided = string.IsNullOrEmpty(filter.color);

            var housesQuery = _context.MushroomHouses
                .AsNoTracking()
                .Where(h =>
                    (isResidentProvided || h.Residents.Any(s => EF.Functions.Like(s.Name, filter.resident))) &&
                    (isMinCapacityProvided || h.Capacity >= filter.minCapacity) &&
                    (isMaxCapacityProvided || h.Capacity <= filter.maxCapacity) &&
                    (isColorProvided || EF.Functions.Like(h.Color.Name, filter.color)));

            if (!housesQuery.Any())
            {
                return new List<MushroomHouseDto>();
            }

            if(!string.IsNullOrWhiteSpace(orderBy))
            {
                housesQuery = OrderUtil<MushroomHouse>.Order(housesQuery, _acceptedParams, orderBy);
            }

            pageSize = Math.Min(pageSize, 100);

            return await housesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToDtoExpression)
                .ToListAsync();
        }

        public async Task<MushroomHouseDto?> GetByIdAsnyc(int id)
        {
            return await _context.MushroomHouses
                .AsNoTracking()
                .Where(h => h.Id == id)
                .Select(ToDtoExpression)
                .FirstOrDefaultAsync();
        }

        public async Task<MushroomHouseDto> InsertAsync(CreateMushroomHouseDto value)
        {
            var message = await CheckForGeneralConstraints(value);
            if (message != null)
            {
                throw new ArgumentException(message);
            }

            var residents = await _context.Smurfs
                .Where(s => value.ResidentIds.Contains(s.Id))
                .ToListAsync();

            var house = ToModel(value, residents);

            await _context.MushroomHouses.AddAsync(house);
            await _context.SaveChangesAsync();

            return ToDto(house);
        }

        public async Task UpdateAsync(UpdateMushroomHouseDto value)
        {
            var  message = await CheckForGeneralConstraints(value);
            if (message != null)
            {
                throw new ArgumentException(message);
            }

            var house = await _context.MushroomHouses.FindAsync(value.Id);
            if (house == null)
            {
                throw new KeyNotFoundException("House not found!");
            }

            await _context.Entry(house)
                .Collection(h => h.Residents)
                .LoadAsync();

            var residents = await _context.Smurfs
                .Where(s => value.ResidentIds.Contains(s.Id))
                .ToListAsync();

            house.Residents = residents;
            house.Capacity = value.Capacity;
            house.AcceptedFoods = value.AcceptedFoods.Select(f => (Food)f).ToList();
            house.Color = ColorDto.ToColor(value.Color);
            house.Motto = value.Motto;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var house = await _context.MushroomHouses.FindAsync(id);
            if (house == null)
            {
                throw new KeyNotFoundException("House not found!");
            }

            var hasResidents = await _context.MushroomHouses
                .Where(h => h.Id == id)
                .SelectMany(h => h.Residents)
                .AnyAsync();

            if (hasResidents)
            {
                throw new InvalidOperationException("This house still has residents in it!");
            }

            _context.MushroomHouses.Remove(house);
            await _context.SaveChangesAsync();
        }

        public async Task AddResidentAsync(int houseId, int smurfId)
        {
            var house = await _context.MushroomHouses.FindAsync(houseId);
            if (house == null)
            {
                throw new KeyNotFoundException("House not found!");
            }

            var smurf = await _context.Smurfs.FindAsync(smurfId);
            if (smurf == null)
            {
                throw new KeyNotFoundException("Smurf not found!");
            }

            if (house.Color.ToArgb() == smurf.FavouriteColor.ToArgb())
            {
                throw new InvalidOperationException("Incompatible colors!");
            }

            if (house.Residents.Contains(smurf))
            {
                throw new InvalidOperationException("This smurf is already a resident!");
            }

            var residentCount = await _context.MushroomHouses
                .Where(l => l.Id == houseId)
                .Select(l => l.Residents.Count)
                .FirstAsync();

            if (residentCount + 1 > house.Capacity)
            {
                throw new InvalidOperationException("This house can't take any more residents!");
            }

            house.Residents.Add(smurf);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveResidentAsync(int houseId, int smurfId)
        {
            var house = await _context.MushroomHouses.FindAsync(houseId);
            if (house == null)
            {
                throw new KeyNotFoundException("House not found!");
            }

            var smurf = await _context.Smurfs.FindAsync(smurfId);
            if (smurf == null)
            {
                throw new KeyNotFoundException("Smurf not found!");
            }

            if (!house.Residents.Contains(smurf))
            {
                throw new KeyNotFoundException("This smurf is not a resident!");
            }

            house.Residents.Remove(smurf);
            await _context.SaveChangesAsync();
        }

        private async Task<string?> CheckForGeneralConstraints(BaseMushroomHouseDto value)
        {
            if(value.Capacity < value.ResidentIds.Count)
            {
                return "The capacity is too small!";
            }

            var residentCount = await _context.Smurfs
                .CountAsync(s => value.ResidentIds.Contains(s.Id));

            if (value.ResidentIds.Count != residentCount)
            {
                return "Unknown smurf!";
            }

            var isHouseColorIncompatible = await _context.Smurfs
                .AnyAsync(s =>
                    value.ResidentIds.Contains(s.Id) &&
                    s.FavouriteColor.ToArgb() == ColorDto.ToColor(value.Color).ToArgb());

            if (isHouseColorIncompatible)
            {
                return "Incompatible colors!";
            }

            return null;
        }

        private Expression<Func<MushroomHouse, MushroomHouseDto>> ToDtoExpression = h => new MushroomHouseDto
        {
            Id = h.Id,
            Capacity = h.Capacity,
            Color = ColorDto.FromColor(h.Color),
            Motto = h.Motto,
            ResidentIds = h.Residents.Select(r => r.Id).ToList(),
            AcceptedFoods = h.AcceptedFoods
        };

        private MushroomHouseDto ToDto(MushroomHouse h) => new MushroomHouseDto
        {
            Id = h.Id,
            Capacity = h.Capacity,
            Color = ColorDto.FromColor(h.Color),
            Motto = h.Motto,
            ResidentIds = h.Residents.Select(r => r.Id).ToList(),
            AcceptedFoods = h.AcceptedFoods
        };

        private MushroomHouse ToModel(BaseMushroomHouseDto value, List<Smurf> residents) => new MushroomHouse
        {
            Residents = residents,
            Capacity = value.Capacity,
            AcceptedFoods = value.AcceptedFoods.Select(f => (Food)f).ToList(),
            Color = ColorDto.ToColor(value.Color),
            Motto = value.Motto
        };
    }
}
