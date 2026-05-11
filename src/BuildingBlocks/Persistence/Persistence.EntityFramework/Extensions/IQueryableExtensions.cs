using Microsoft.EntityFrameworkCore;
using Persistence.Base.Models;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Persistence.EntityFramework.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> Sort<T>(this IQueryable<T> entities, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return entities;

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty == null)
                continue;

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        if (string.IsNullOrWhiteSpace(orderQuery))
            return entities;

        return entities.OrderBy(orderQuery);
    }

    private static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderByProperty)
    {
        string command = orderByProperty.EndsWith(" descending") ? "OrderByDescending" : "OrderBy";
        var propertyName = orderByProperty.Split(' ')[0];
        var type = typeof(T);
        var property = type.GetProperty(propertyName);
        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
            source.Expression, Expression.Quote(orderByExpression));
        return source.Provider.CreateQuery<T>(resultExpression);
    }

    public static IQueryable<T> Search<T>(this IQueryable<T> entities, string searchTerm, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(propertyName))
            return entities;

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (property == null || property.PropertyType != typeof(string))
            throw new ArgumentException($"Property '{propertyName}' does not exist or is not a string property.");

        var parameter = Expression.Parameter(typeof(T), "e");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        var toLowerExpression = Expression.Call(propertyAccess, toLowerMethod);
        var searchTermExpression = Expression.Constant(lowerCaseTerm);
        var containsExpression = Expression.Call(toLowerExpression, containsMethod, searchTermExpression);

        var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

        return entities.Where(lambda);
    }

    public static async Task<OffSetPagedList<T>> ToPagedListAsync<T>(
    this IQueryable<T> source,
    int pageNumber,
    int pageSize)
    where T : class
    {
        var count = await source.CountAsync();

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new OffSetPagedList<T>(items, count, pageSize, pageNumber);
    }
}