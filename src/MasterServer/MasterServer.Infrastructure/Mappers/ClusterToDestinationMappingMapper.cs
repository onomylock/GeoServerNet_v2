using MasterServer.Domain.Entities;

namespace MasterServer.Infrastructure.Mappers;

public static class ClusterToDestinationMappingMapper
{
    public static ClusterToDestinationMapping ToClusterToDestinationMapping(Guid clusterId, Guid destinationId)
    {
        return new ClusterToDestinationMapping
        {
            EntityLeftId = clusterId,
            EntityRightId = destinationId
        };
    }
}