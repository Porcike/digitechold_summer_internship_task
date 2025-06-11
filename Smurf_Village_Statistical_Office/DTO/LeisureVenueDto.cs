namespace Smurf_Village_Statistical_Office.DTO
{
    public record LeisureVenueDto(
        int Id,
        string Name,
        int Capacity,
        List<int> MemberIds,
        BrandDto AcceptedBrand);
}
