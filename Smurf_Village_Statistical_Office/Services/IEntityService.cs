namespace Smurf_Village_Statistical_Office.Services
{
    public interface IEntityService<Dto, CreateDto, DtoFilter> 
        where Dto : class 
        where CreateDto : class
        where DtoFilter : class
    {
        Task<IReadOnlyCollection<Dto>> GetAllAsync(DtoFilter filter);
        Task<Dto?> GetByIdAsnyc(int id);
        Task<Dto> InsertAsync(CreateDto value);
    }
}
