using System.Drawing;

namespace Smurf_Village_Statistical_Office.DTO.MushroomHouseDtos
{
    public abstract record BaseMushroomHouseDto
    {
        public int Capacity { get; init; }
        public string Motto { get; init; } = string.Empty;
        public List<int> ResidentIds { get; init; } = new List<int>();
        public List<int> AcceptedFoods { get; init; } = new List<int>();
        public Color Color { get; init; }

    }
}
