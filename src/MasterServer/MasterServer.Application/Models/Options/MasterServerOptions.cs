using Shared.Common.Models.Options;

namespace MasterServer.Application.Models.Options;

public class MasterServerOptions : CommonServiceOptions
{
    /// <summary>
    ///     CORS allowed origins
    /// </summary>
    public string[] CorsAllowedOrigins { get; set; }
}