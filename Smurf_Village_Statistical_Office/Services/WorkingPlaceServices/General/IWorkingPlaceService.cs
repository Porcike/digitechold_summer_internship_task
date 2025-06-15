using Smurf_Village_Statistical_Office.DTO.WorkingPlaceDtos;

namespace Smurf_Village_Statistical_Office.Services.WorkingPlaceServices.General
{
    public interface IWorkingPlaceService
    {
        Task<IReadOnlyCollection<WorkingPlaceDto>> GetAllAsync(WorkingPlaceFilterDto filter, int page, int pageSize, string? orderBy);
        Task<WorkingPlaceDto?> GetByIdAsnyc(int id);
        Task<WorkingPlaceDto> InsertAsync(CreateWorkingPlaceDto value);
        Task UpdateAsync(UpdateWorkingPlaceDto value);
        Task DeleteAsync(int id);
        Task AddEmployeeAsync(int workplaceId, int smurfId);
        Task RemoveEmployeeAsync(int workplaceId, int smurfId);
    }
}
