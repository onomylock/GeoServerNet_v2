using MasterServer.Domain.Entities;

namespace MasterServer.Infrastructure.Mappers;

public static class ClusterToNodeMappingMapper
{
    public static ClusterToNodeMapping ToClusterToNodeMapping(Guid clusterId, Guid nodeId)
    {
        return new ClusterToNodeMapping
        {
            EntityLeftId = clusterId,
            EntityRightId = nodeId
        };
    }
}