namespace Smurf_Village_Statistical_Office.DTO.Filters
{
    public record MushroomHouseFilterDto(
        string? resident,
        int? minCapacity,
        int? maxCapacity,
        string? color);
}
