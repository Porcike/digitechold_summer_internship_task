namespace Smurf_Village_Statistical_Office.DTO.WorkingPlaceDtos
{
    public abstract record BaseWorkingPlaceDto
    {
        public string Name { get; init; } = string.Empty;
        public List<int> AcceptedJobs { get; init; } = new List<int>();
        public List<int> EmployeeIds { get; init; } = new List<int>();
    }
}
