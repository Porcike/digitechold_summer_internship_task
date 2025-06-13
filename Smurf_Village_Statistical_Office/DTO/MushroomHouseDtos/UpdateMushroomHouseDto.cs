namespace Smurf_Village_Statistical_Office.DTO.MushroomHouseDtos
{
    public record UpdateMushroomHouseDto : BaseMushroomHouseDto
    {
        public required int Id { get; init; }
    }
}
