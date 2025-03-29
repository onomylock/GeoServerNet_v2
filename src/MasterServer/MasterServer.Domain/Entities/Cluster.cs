using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Cluster : EntityBase
{
    public string Alias { get; set; }
    public string LoadBalancingPolicy { get; set; }
    public int HealthCheckInterval { get; set; } 
    public string HealthCheckPath { get; set; }
    public string RouteAlias { get; set; }
}