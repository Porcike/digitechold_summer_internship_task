using Smurf_Village_Statistical_Office.DTO.MushroomHouseDtos;

namespace Smurf_Village_Statistical_Office.Services.MushroomHouseServices.General
{
    public interface IMushroomHouseService
    {
        Task<IReadOnlyCollection<MushroomHouseDto>> GetAllAsync(MushroomHouseFilterDto filter, int page, int pageSize, string? orderBy);
        Task<MushroomHouseDto?> GetByIdAsnyc(int id);
        Task<MushroomHouseDto> InsertAsync(CreateMushroomHouseDto value);
        Task UpdateAsync(UpdateMushroomHouseDto value);
        Task DeleteAsync(int id);
        Task AddResidentAsync(int houseId, int smurfId);
        Task RemoveResidentAsync(int houseId, int smurfId);
    }
}
