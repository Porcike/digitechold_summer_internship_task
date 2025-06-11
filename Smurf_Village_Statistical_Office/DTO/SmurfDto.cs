namespace Smurf_Village_Statistical_Office.DTO
{
    public record SmurfDto
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public int Age { get; init; }
        public required JobDto Job { get; init; }
        public required FoodDto FavouriteFood { get; init; }
        public required BrandDto FavouriteBrand { get; init; }
        public required ColorDto FavouriteColor { get; init; }
    }
}
