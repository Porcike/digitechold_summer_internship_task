namespace Smurf_Village_Statistical_Office.DTO.FoodDtos
{
    public record FoodDto
    {
        public int Id { get; init; }
        public required string Name { get; init; }
    }
}
