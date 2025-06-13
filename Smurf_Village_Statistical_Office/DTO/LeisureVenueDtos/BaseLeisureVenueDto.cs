namespace Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos
{
    public abstract record BaseLeisureVenueDto
    {
        public string Name { get; init; } = string.Empty;
        public int Capacity { get; init; }
        public int AcceptedBrand { get; init; }
        public List<int> MemberIds { get; init; } = new List<int>();
    }
}
