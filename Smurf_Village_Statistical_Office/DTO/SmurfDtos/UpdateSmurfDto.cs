namespace Smurf_Village_Statistical_Office.DTO.SmurfDtos
{
    public record UpdateSmurfDto : BaseSmurfDto
    {
        public required int Id { get; init; }
    }
}
