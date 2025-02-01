using System.ComponentModel.DataAnnotations;

namespace Shared.Common.Models.Options;

public class CommonServiceOptions
{
    [Required] [Range(0, int.MaxValue)] public int RedisGenericDatabase { get; set; }

    /// <summary>
    ///     Used to for maintenance. System access level
    /// </summary>
    [Required]
    [MinLength(1)]
    public string SystemAccessToken { get; set; }
}