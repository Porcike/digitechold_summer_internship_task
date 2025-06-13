namespace Smurf_Village_Statistical_Office.DTO.MushroomHouseDtos
{
    public record MushroomHouseFilterDto(
        string? resident,
        int? minCapacity,
        int? maxCapacity,
        string? color);
}
