using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Mappers;

public static class ClusterMapper
{
    public static async Task<ClusterReadResultDto> ToClusterReadResultDto(Cluster cluster, IClusterToNodeMappingEntityService clusterToNodeMappingEntityService, CancellationToken cancellationToken = default)
    {
        return await ToClusterReadResultDto(cluster, clusterToNodeMappingEntityService, cancellationToken);
    }

    private static async Task<ClusterReadResultDto> ToClusterReadResultOutDto(Cluster cluster,
        IClusterToNodeMappingEntityService clusterToNodeMappingEntityService, CancellationToken cancellationToken = default)
    {
        return new ClusterReadResultDto
        {
            Id = cluster.Id,
            CreatedAt = cluster.CreatedAt,
            UpdatedAt = cluster.UpdatedAt,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            UserId = cluster.UserId,
            NodeIds = clusterToNodeMappingEntityService is { } ? (await clusterToNodeMappingEntityService.GetCollection(PageModel.Max,
                    query => query.Where(_ => _.EntityLeftId == cluster.Id), true, cancellationToken)).entities
                .Select(_ => _.EntityLeftId).ToArray()
            : null,
        };
    }

    public static async Task<ClusterReadCollectionResultDto> ToClusterReadCollectionResultDto(
        (int total, IReadOnlyCollection<Cluster> entities) targetClusters,
        IClusterToNodeMappingEntityService clusterToNodeMappingEntityService,
        CancellationToken cancellationToken = default)
    {
        var items = new List<ClusterReadResultDto>();
        
        foreach(var cluster in targetClusters.entities)
            items.Add(await ToClusterReadResultDto(cluster, clusterToNodeMappingEntityService, cancellationToken));

        return new ClusterReadCollectionResultDto
        {
            Total = targetClusters.total,
            Items = items.ToArray()
        };
    }
}