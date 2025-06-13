namespace Smurf_Village_Statistical_Office.DTO.SmurfDtos
{
    public record SmurfFilterDto(
        string? name,
        int? minAge,
        int? maxAge,
        string? job, 
        string? favouriteFood,
        string? favouriteBrand,
        string? favouriteColor);
}
