using Yarp.ReverseProxy.Configuration;

namespace ReverseProxy.Application.Services;

public interface ICustomInMemoryProxyConfigProvider : IProxyConfigProvider
{
    public Task Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters);
    public Task Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters, string revisionId);
    
}