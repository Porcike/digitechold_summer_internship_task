using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.General
{
    public abstract class BaseEntityService<TEntity, TDto, TCreateDto, TUpdateDto, TDtoFilter>
        where TEntity : class
        where TDto : class
        where TCreateDto : class
        where TUpdateDto : class
        where TDtoFilter : class
    {
        protected List<string> AcceptedParams { get; init; } = new List<string>();

        public abstract Task<IReadOnlyCollection<TDto>> GetAllAsync(TDtoFilter filter, int page, int pageSize, string? orderBy);
        public abstract Task<TDto?> GetByIdAsnyc(int id);
        public abstract Task<TDto> InsertAsync(TCreateDto value);
        public abstract Task UpdateAsync(TUpdateDto value);
        public abstract Task DeleteAsync(int id);

        protected IQueryable<TEntity> Order(IQueryable<TEntity> query, string orderBy)
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

                if (AcceptedParams.Contains(propertyName))
                {
                    query = query.OrderByProperyName(propertyName, foundParams.Count == 0, param.EndsWith(" desc"));
                    foundParams.Add(propertyName);
                }

                if (foundParams.Count == AcceptedParams.Count)
                {
                    break;
                }
            }

            return query;
        }
    }
}
