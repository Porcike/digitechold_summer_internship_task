namespace Smurf_Village_Statistical_Office.DTO
{
    public record SmurfDto(
        int Id, 
        string Name, 
        int Age, 
        JobDto Job,
        FoodDto FavouriteFood,
        BrandDto FavouriteBrand,
        ColorDto FavouriteColor);
}
