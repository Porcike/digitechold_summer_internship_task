namespace Smurf_Village_Statistical_Office.Services.General
{
    public interface IEntityService<TDto, TCreateDto, TUpdateDto, TDtoFilter> 
        where TDto : class 
        where TCreateDto : class
        where TUpdateDto : class
        where TDtoFilter : class
    {
        Task<IReadOnlyCollection<TDto>> GetAllAsync(TDtoFilter filter);
        Task<TDto?> GetByIdAsnyc(int id);
        Task<TDto> InsertAsync(TCreateDto value);
        Task UpdateAsync(TUpdateDto value);
        Task DeleteAsync(int id);
    }
}
