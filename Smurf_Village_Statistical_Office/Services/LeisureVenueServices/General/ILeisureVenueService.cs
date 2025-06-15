using Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos;

namespace Smurf_Village_Statistical_Office.Services.LeisureVenueServices.General
{
    public interface ILeisureVenueService
    {
        Task<IReadOnlyCollection<LeisureVenueDto>> GetAllAsync(LeisureVenueFilterDto filter, int page, int pageSize, string? orderBy);
        Task<LeisureVenueDto?> GetByIdAsnyc(int id);
        Task<LeisureVenueDto> InsertAsync(CreateLeisureVenueDto value);
        Task UpdateAsync(UpdateLeisureVenueDto value);
        Task DeleteAsync(int id);
        Task AddMemberAsync(int venueId, int smurfId);
        Task RemoveMemberAsync(int venueId, int smurfId);
    }
}
