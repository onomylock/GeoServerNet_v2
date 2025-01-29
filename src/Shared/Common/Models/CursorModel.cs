using System.ComponentModel.DataAnnotations;

namespace Shared.Common.Models;

public record CursorModel
{
    /// <summary>
    ///     Property of an entity to use as a cursor
    /// </summary>
    /// <remarks>Must use properties with unique values</remarks>
    public string By { get; set; } = "CreatedAt";

    /// <summary>
    ///     Cursor position
    /// </summary>
    public string At { get; set; }

    /// <summary>
    ///     Reverses an order by property of an entity that is used as a cursor
    /// </summary>
    public bool Reverse { get; set; }

    [Range(1, 500)] public int PageSize { get; set; } = 500;

    /// <summary>
    ///     Special value that must not be used, only in rare cases
    /// </summary>
    public static CursorModel Full => new()
        { PageSize = int.MaxValue };

    public static CursorModel Max(string by, string at)
    {
        return new CursorModel
        {
            By = by,
            At = at,
            PageSize = 500
        };
    }
}