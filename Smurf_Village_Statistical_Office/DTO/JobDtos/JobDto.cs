namespace Smurf_Village_Statistical_Office.DTO.JobDtos
{
    public record JobDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
    }
}
