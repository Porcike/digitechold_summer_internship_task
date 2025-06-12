namespace Smurf_Village_Statistical_Office.DTO
{
    public record CreateLeisureVenueDto
    {
        public required string Name { get; init; }
        public required int Capacity { get; init; }
        public List<int> MemberIds { get; init; } = new List<int>();
        public required int AcceptedBrand { get; init; }
    }
}
