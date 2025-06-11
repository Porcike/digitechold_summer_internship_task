using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.DTO
{
    public record LeisureVenueDto
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public int Capacity { get; init; }
        public required List<int> MemberIds { get; init; }
        public Brand AcceptedBrand { get; init; }
    }
}
