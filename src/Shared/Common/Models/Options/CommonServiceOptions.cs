using System.ComponentModel.DataAnnotations;

namespace Shared.Common.Models.Options;

public class CommonServiceOptions
{
    /// <summary>
    ///     Used to for maintenance. System access level
    /// </summary>
    [Required]
    [MinLength(1)]
    public string SystemAccessToken { get; set; }
}