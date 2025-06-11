using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.DTO
{
    public record SmurfDto
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public int Age { get; init; }
        public required Job Job { get; init; }
        public required Food FavouriteFood { get; init; }
        public required Brand FavouriteBrand { get; init; }
        public required ColorDto FavouriteColor { get; init; }
    }
}
