namespace Smurf_Village_Statistical_Office.Services
{
    public interface IEntityService<TDto, TDtoFilter> 
        where TDto : class 
        where TDtoFilter : class
    {
        Task<IEnumerable<TDto>> GetAllAsync(TDtoFilter filter);
        Task<TDto?> GetByIdAsnyc(int id);
    }
}
