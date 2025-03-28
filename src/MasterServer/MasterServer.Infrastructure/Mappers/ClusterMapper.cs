using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Models.Dto.Cluster.Notification;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Mappers;

public static class ClusterMapper
{
    public static async Task<ClusterReadResultDto> ToClusterReadResultDto(Cluster cluster,
        IClusterToNodeMappingEntityService clusterToNodeMappingEntityService,
        CancellationToken cancellationToken = default)
    {
        return await ToClusterReadResultOutDto(cluster, clusterToNodeMappingEntityService, cancellationToken);
    }

    public static async Task<ClusterReadCollectionResultDto> ToClusterReadCollectionResultDto(
        (int total, IReadOnlyCollection<Cluster> entities) targetClusters,
        IClusterToNodeMappingEntityService clusterToNodeMappingEntityService,
        CancellationToken cancellationToken = default)
    {
        var items = new List<ClusterReadResultDto>();

        foreach (var cluster in targetClusters.entities)
            items.Add(await ToClusterReadResultDto(cluster, clusterToNodeMappingEntityService, cancellationToken));

        return new ClusterReadCollectionResultDto
        {
            Total = targetClusters.total,
            Items = items.ToArray()
        };
    }

    public static ClusterReadNotificationDto ToClusterReadNotificationDto(Cluster cluster,
        IReadOnlyCollection<Node> nodes)
    {
        return new ClusterReadNotificationDto
        {
            ClusterId = cluster.Id,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            Nodes = nodes.Select(NodeMapper.ToNodeReadResultBase).ToArray()
        };
    }
    
    public static ClusterCreatedNotificationDto ToClusterCreatedNotificationDto(Cluster cluster,
        IReadOnlyCollection<Node> nodes)
    {
        return new ClusterCreatedNotificationDto
        {
            ClusterId = cluster.Id,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            Nodes = nodes.Select(NodeMapper.ToNodeReadResultBase).ToArray()
        };
    }
    
    public static ClusterDeletedNotificationDto ToClusterDeletedNotificationDto(Cluster cluster)
    {
        return new ClusterDeletedNotificationDto
        {
            ClusterId = cluster.Id,
        };
    }

    private static async Task<ClusterReadResultDto> ToClusterReadResultOutDto(Cluster cluster,
        IClusterToNodeMappingEntityService clusterToNodeMappingEntityService,
        CancellationToken cancellationToken = default)
    {
        return new ClusterReadResultDto
        {
            Id = cluster.Id,
            CreatedAt = cluster.CreatedAt,
            UpdatedAt = cluster.UpdatedAt,
            ClusterId = cluster.Id,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            NodeIds = clusterToNodeMappingEntityService is not null
                ? (await clusterToNodeMappingEntityService.GetCollection(PageModel.Max,
                    query => query.Where(_ => _.EntityLeftId == cluster.Id), true, cancellationToken)).entities
                .Select(_ => _.EntityLeftId).ToArray()
                : null
        };
    }
}