namespace Smurf_Village_Statistical_Office.DTO
{
    public record CreateSmurfDto
    {
        public required string Name { get; init; }
        public required int Age { get; init; }
        public required int Job { get; init; }
        public required int FavouriteFood { get; init; }
        public required int FavouriteBrand { get; init; }
        public required ColorDto FavouriteColor { get; init; }
    }
}
