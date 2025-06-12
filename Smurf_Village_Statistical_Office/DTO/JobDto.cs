namespace Smurf_Village_Statistical_Office.DTO
{
    public record JobDto
    {
        public int Id { get; init; }
        public required string Name { get; init; }
    }
}
