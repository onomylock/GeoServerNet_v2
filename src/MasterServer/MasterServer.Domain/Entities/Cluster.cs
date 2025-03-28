using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Cluster : EntityBase
{
    public string LoadBalancingPolicy { get; set; }
}