using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.DTO
{
    public record LeisureVenueDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required int Capacity { get; init; }
        public required List<int> MemberIds { get; init; }
        public required Brand AcceptedBrand { get; init; }
    }
}
