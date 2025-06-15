using Smurf_Village_Statistical_Office.DTO.SmurfDtos;

namespace Smurf_Village_Statistical_Office.Services.SmurfServices.General
{
    public interface ISmurfService
    {
        Task<IReadOnlyCollection<SmurfDto>> GetAllAsync(SmurfFilterDto filter, int page, int pageSize, string? orderBy);
        Task<SmurfDto?> GetByIdAsnyc(int id);
        Task<SmurfDto> InsertAsync(CreateSmurfDto value);
        Task UpdateAsync(UpdateSmurfDto value);
        Task DeleteAsync(int id);
    }
}
