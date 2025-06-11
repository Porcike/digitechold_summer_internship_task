namespace Smurf_Village_Statistical_Office.DTO
{
    public record WorkingPlaceDto(
        int Id,
        string Name,
        List<int> EmployeeIds,
        List<JobDto> AcceptedJobs);
}
