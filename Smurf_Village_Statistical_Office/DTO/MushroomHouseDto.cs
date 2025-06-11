namespace Smurf_Village_Statistical_Office.DTO
{
    public record MushroomHouseDto(
        int Id,
        List<int> ResidentIds,
        int Capacity,
        List<FoodDto> AcceptedFoods,
        ColorDto Color,
        string Motto);
}
