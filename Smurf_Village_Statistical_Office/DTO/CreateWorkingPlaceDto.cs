namespace Smurf_Village_Statistical_Office.DTO
{
    public record CreateWorkingPlaceDto
    {
        public required string Name { get; init; }
        public List<int> AcceptedJobs { get; init; } = new List<int>();
        public List<int> EmployeeIds { get; init; } = new List<int>();
    }
}
