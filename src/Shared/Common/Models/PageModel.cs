using System.ComponentModel.DataAnnotations;

namespace Shared.Common.Models;

public record PageModel
{
    [Range(1, int.MaxValue)] public int Page { get; set; } = 1;

    [Range(1, 500)] public int PageSize { get; set; } = 10;

    /// <summary>
    ///     Special value used to count entities for a query, without materialization
    /// </summary>
    /// <remarks>Only for internal use</remarks>
    public static PageModel Count => new()
        { Page = -1, PageSize = -1 };

    public static PageModel Max => new()
        { Page = 1, PageSize = 500 };

    /// <summary>
    ///     Special value that must not be used, only in rare cases
    /// </summary>
    public static PageModel Full => new()
        { Page = 1, PageSize = int.MaxValue };
}