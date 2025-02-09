using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Cluster : EntityBase
{
    public Guid UserId { get; set; }
    public string LoadBalancingPolicy { get; set; }
}