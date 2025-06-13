using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.DTO.WorkingPlaceDtos
{
    public record WorkingPlaceDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required List<Job> AcceptedJobs { get; init; }
        public required List<int> EmployeeIds { get; init; }
    }
}
