using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> FullTextSearch<T>(
        this IQueryable<T> query, 
        string searchTerm, 
        Expression<Func<T, string>> propertySelector)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Invoke(propertySelector, parameter);
        
        var method = typeof(NpgsqlDbFunctionsExtensions)
            .GetMethod(nameof(NpgsqlDbFunctionsExtensions.ILike))!
            .MakeGenericMethod(typeof(string));

        var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));
        var searchPattern = Expression.Constant($"%{searchTerm}%");
        var iLikeExp = Expression.Call(method, efFunctions, property, searchPattern);
        
        var lambda = Expression.Lambda<Func<T, bool>>(iLikeExp, parameter);
        
        return query.Where(lambda);
    }
}