namespace Smurf_Village_Statistical_Office.DTO
{
    public record ColorDto
    {
        public required string Name { get; init; }
        public int Red { get; init; }
        public int Green { get; init; }
        public int Blue { get; init; }
        public int Alpha { get; init; }
    }
}
