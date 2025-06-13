using Smurf_Village_Statistical_Office.DTO.ColorDtos;

namespace Smurf_Village_Statistical_Office.DTO.SmurfDtos
{
    public abstract record BaseSmurfDto
    {
        public string Name { get; init; } = string.Empty;
        public int Age { get; init; }
        public int Job { get; init; }
        public int FavouriteFood { get; init; }
        public int FavouriteBrand { get; init; }
        public ColorDto FavouriteColor { get; init; } = new ColorDto
        {
            Red = 0,
            Green = 0,
            Blue = 0,
            Alpha = 255,
        };
    }
}
