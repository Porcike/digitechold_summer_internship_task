using System.Linq.Expressions;

namespace Smurf_Village_Statistical_Office.Utils
{
    public static class QueryableExtension
    {
        public static IOrderedQueryable<TEntity> OrderByProperyName<TEntity>(
            this IQueryable<TEntity> query,
            string propertyName,
            bool firstParameter,
            bool descending)
        {
            var param = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(param, propertyName);
            var lambda = Expression.Lambda(property, param);

            var methodName = firstParameter
                ? (descending ? "OrderByDescending" : "OrderBy")
                : (descending ? "ThenByDescending" : "ThenBy");

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TEntity), property.Type);

            return (IOrderedQueryable<TEntity>)method.Invoke(null, [query, lambda])!;
        }
    }
}
