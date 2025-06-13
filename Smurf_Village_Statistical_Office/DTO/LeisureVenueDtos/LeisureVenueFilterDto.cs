namespace Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos
{
    public record LeisureVenueFilterDto(
        string? name,
        int? minCapacity,
        int? maxCapacity,
        string? member,
        string? brand);
}
