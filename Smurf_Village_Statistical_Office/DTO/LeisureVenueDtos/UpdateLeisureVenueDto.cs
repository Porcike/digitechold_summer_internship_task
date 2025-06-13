namespace Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos
{
    public record UpdateLeisureVenueDto : BaseLeisureVenueDto
    {
        public required int Id { get; init; }
    }
}
