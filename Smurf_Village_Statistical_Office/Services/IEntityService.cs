namespace Smurf_Village_Statistical_Office.Services
{
    public interface IEntityService<TDto, TDtoFilter>
    {
        Task<IEnumerable<TDto>> GetAllAsync(TDtoFilter filter);
        Task<TDto> GetByIdAsnyc(int id);
    }
}
