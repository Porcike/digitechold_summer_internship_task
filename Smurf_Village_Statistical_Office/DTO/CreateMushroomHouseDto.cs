namespace Smurf_Village_Statistical_Office.DTO
{
    public record CreateMushroomHouseDto
    {
        public required int Capacity { get; init; }
        public required ColorDto Color { get; init; }
        public string Motto { get; init; } = string.Empty;
        public List<int> ResidentIds { get; init; } = new List<int>();
        public List<int> AcceptedFoods { get; init; } = new List<int>();
    }
}
