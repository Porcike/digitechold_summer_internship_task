﻿using Smurf_Village_Statistical_Office.DTO.ColorDtos;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.DTO.MushroomHouseDtos
{
    public record MushroomHouseDto
    {
        public required int Id { get; init; }
        public required int Capacity { get; init; }
        public required ColorDto Color { get; init; }
        public required string Motto { get; init; }
        public required List<int> ResidentIds { get; init; }
        public required List<Food> AcceptedFoods { get; init; }
    }
}
