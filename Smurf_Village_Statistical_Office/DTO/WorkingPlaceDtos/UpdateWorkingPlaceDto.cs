namespace Smurf_Village_Statistical_Office.DTO.WorkingPlaceDtos
{
    public record UpdateWorkingPlaceDto : BaseWorkingPlaceDto
    {
        public required int Id { get; init; }
    }
}
