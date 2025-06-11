namespace Smurf_Village_Statistical_Office.DTO
{
    public record MushroomHouseDto
    {
        public int Id { get; init; }
        public required int Capacity { get; init; }
        public required ColorDto Color { get; init; }
        public required string Motto { get; init; }
        public required List<int> ResidentIds { get; init; }
    }
}
