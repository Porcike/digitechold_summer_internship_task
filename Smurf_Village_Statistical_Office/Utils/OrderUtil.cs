namespace Smurf_Village_Statistical_Office.Utils
{
    public static class OrderUtil<TEntity>
    {
        public static IQueryable<TEntity> Order(IQueryable<TEntity> query, string[] acceptedParams, string orderBy)
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
