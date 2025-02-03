using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Models;
using Shared.Domain.Entity.Base;

namespace Shared.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<(int total, IQueryable<T> query)> GetPage<T>(this IQueryable<T> query, PageModel model, CancellationToken cancellationToken = default) where T : class
    {
        if (model == PageModel.Count)
            return (await query.CountAsync(cancellationToken), null);

        return (
            await query.CountAsync(cancellationToken),
            query
                .Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
        );
    }

    public static async Task<(string prev, IQueryable<T> query, string next)> GetPage<T>(this IQueryable<T> query, CursorModel model, CancellationToken cancellationToken = default)
        where T : EntityBase
    {
        if (string.IsNullOrWhiteSpace(model.At))
            model.At = model.Reverse
                ? ToCursorString(GetPropertyValue(await query.OrderBy($"{model.By} desc").FirstOrDefaultAsync(cancellationToken), model.By))
                : ToCursorString(GetPropertyValue(await query.OrderBy($"{model.By}").FirstOrDefaultAsync(cancellationToken), model.By));

        if (model.Reverse)
        {
            var queryAfter = query
                .Where($"{model.By} <= @0", model.At)
                .OrderBy($"{model.By} desc");
            var queryAfterPage = queryAfter.Take(model.PageSize);

            var queryBefore = query
                .Where($"{model.By} > @0", model.At)
                .OrderBy($"{model.By}");
            var queryBeforePage = queryBefore
                .Take(model.PageSize);

            return (
                ToCursorString(GetPropertyValue(await queryBeforePage.LastOrDefaultAsync(cancellationToken), model.By)),
                queryAfterPage,
                ToCursorString(GetPropertyValue(await queryAfterPage.LastOrDefaultAsync(cancellationToken), model.By))
            );
        }
        else
        {
            var queryAfter = query
                .Where($"{model.By} >= @0", model.At)
                .OrderBy($"{model.By}");
            var queryAfterPage = queryAfter.Take(model.PageSize);

            var queryBefore = query
                .Where($"{model.By} < @0", model.At)
                .OrderBy($"{model.By} desc");
            var queryBeforePage = queryBefore
                .Take(model.PageSize);

            return (
                ToCursorString(GetPropertyValue(await queryBeforePage.LastOrDefaultAsync(cancellationToken), model.By)),
                queryAfterPage,
                ToCursorString(GetPropertyValue(await queryAfterPage.LastOrDefaultAsync(cancellationToken), model.By))
            );
        }
    }

    private static object GetPropertyValue(object obj, string propertyName)
    {
        if (obj == null)
            return null;

        // Get the type of the object
        var type = obj.GetType();

        // Get the PropertyInfo object for the specified property
        var propertyInfo = type.GetProperty(propertyName);

        if (propertyInfo == null) throw new ArgumentException($"Property '{propertyName}' not found on type '{type.FullName}'.");

        // Get the value of the property
        return propertyInfo.GetValue(obj);
    }

    private static string ToCursorString(object obj)
    {
        if (obj == null)
            return null;

        if (obj is DateTimeOffset dateTimeOffset)
            return dateTimeOffset.ToString("o");

        if (obj is DateTime dateTime)
            return dateTime.ToString("o");

        return obj.ToString();
    }
}