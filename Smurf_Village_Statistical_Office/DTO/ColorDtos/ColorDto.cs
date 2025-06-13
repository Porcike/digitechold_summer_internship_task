namespace Smurf_Village_Statistical_Office.DTO.ColorDtos
{
    public record ColorDto
    {
        public required byte Red { get; init; }
        public required byte Green { get; init; }
        public required byte Blue { get; init; }
        public required byte Alpha { get; init; }
    }
}
