using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Common.Helpers;

public static class ServiceCollectionHelpers
{
    /// <summary>
    ///     Add authorization with custom provider
    /// </summary>
    /// <remarks>https://stackoverflow.com/questions/59083989/dependency-injection-on-authorization-policy</remarks>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TDependency>(
        this IServiceCollection services,
        Action<AuthorizationOptions, TDependency> configure
    ) where TDependency : class
    {
        services.AddOptions<AuthorizationOptions>().Configure(configure);
        return services.AddAuthorization();
    }

    /// <summary>
    ///     Add request timeouts with custom provider
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddRequestTimeouts<TDependency>(
        this IServiceCollection services,
        Action<RequestTimeoutOptions, TDependency> configure
    ) where TDependency : class
    {
        services.AddOptions<RequestTimeoutOptions>().Configure(configure);
        return services.AddRequestTimeouts();
    }
}