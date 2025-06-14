using Smurf_Village_Statistical_Office.DTO.LeisureVenueDtos;
using Smurf_Village_Statistical_Office.Services.General;

namespace Smurf_Village_Statistical_Office.Services.LeisureVenueServices.General
{
    public interface ILeisureVenueService : IEntityService<
        LeisureVenueDto, 
        CreateLeisureVenueDto, 
        UpdateLeisureVenueDto,
        LeisureVenueFilterDto>
    {
        Task AddMemberAsync(int venueId, int smurfId);
        Task RemoveMemberAsync(int venueId, int smurfId);
    }
}
