using System.ComponentModel.DataAnnotations;
using Shared.Common.Models;
using Shared.Common.Models.Options;

namespace MasterServer.Application.Models.Options;

public class MasterServiceOptions : CommonServiceOptions
{
    /// <summary>
    ///     HTTP Secure Cookies
    /// </summary>
    public bool SecureCookies { get; set; }

    public string CookiesDomain { get; set; }

    /// <summary>
    ///     CORS allowed origins
    /// </summary>
    public string[] CorsAllowedOrigins { get; set; }

    [Required] public UriData UserServiceChannel { get; set; }
}