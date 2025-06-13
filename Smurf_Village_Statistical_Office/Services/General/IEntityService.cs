namespace Smurf_Village_Statistical_Office.Services.General
{
    public interface IEntityService<Dto, CreateDto, UpdateDto, DtoFilter> 
        where Dto : class 
        where CreateDto : class
        where UpdateDto : class
        where DtoFilter : class
    {
        Task<IReadOnlyCollection<Dto>> GetAllAsync(DtoFilter filter);
        Task<Dto?> GetByIdAsnyc(int id);
        Task<Dto> InsertAsync(CreateDto value);
        Task UpdateAsync(UpdateDto value);
        Task DeleteAsync(int id);
    }
}
