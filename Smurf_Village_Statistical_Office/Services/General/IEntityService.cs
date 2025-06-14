using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.General
{
    public interface IEntityService<TEntity, TDto, TCreateDto, TUpdateDto, TDtoFilter>
        where TEntity : class
        where TDto : class 
        where TCreateDto : class
        where TUpdateDto : class
        where TDtoFilter : class
    {
        Task<IReadOnlyCollection<TDto>> GetAllAsync(TDtoFilter filter, int page, int pageSize, string? orderBy);
        Task<TDto?> GetByIdAsnyc(int id);
        Task<TDto> InsertAsync(TCreateDto value);
        Task UpdateAsync(TUpdateDto value);
        Task DeleteAsync(int id);

        static IQueryable<TEntity> Order(IQueryable<TEntity> query, string[] acceptedParams, string orderBy)
        {
            var orderParams = orderBy.Trim().Split(',');
            var foundParams = new HashSet<string>();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propertyName = param.Trim().Split(" ")[0].ToLower();
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    continue;
                }

                propertyName = char.ToUpper(propertyName[0]) + propertyName[1..];
                if (foundParams.Contains(propertyName))
                {
                    continue;
                }

                if (acceptedParams.Contains(propertyName))
                {
                    query = query.OrderByProperyName(propertyName, foundParams.Count == 0, param.EndsWith(" desc"));
                    foundParams.Add(propertyName);
                }

                if (foundParams.Count == acceptedParams.Length)
                {
                    break;
                }
            }

            return query;
        }
    }
}
