using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Models.Dto.Cluster.Notification;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterCreateCommand;
using MasterServer.Infrastructure.Handlers.Cluster.Notifications.ClusterCreatedNotification;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Mappers;

public static class ClusterMapper
{
    public static Cluster FromClusterCreateCommand(ClusterCreateCommand command)
    {
        return new Cluster
        {
            Alias = command.Alias,
            LoadBalancingPolicy = command.LoadBalancingPolicy,
            HealthCheckInterval = command.HealthCheckInterval,
            HealthCheckPath = command.HealthCheckPath,
            RouteAlias = command.RouteAlias,
        };
    }
    
    public static async Task<ClusterReadResultDto> ToClusterReadResultDto(Cluster cluster,
        IClusterToDestinationMappingEntityService clusterToDestinationMappingEntityService,
        CancellationToken cancellationToken = default)
    {
        return await ToClusterReadResultOutDto(cluster, clusterToDestinationMappingEntityService, cancellationToken);
    }

    public static async Task<ClusterReadCollectionResultDto> ToClusterReadCollectionResultDto(
        (int total, IReadOnlyCollection<Cluster> entities) targetClusters,
        IClusterToDestinationMappingEntityService clusterToDestinationMappingEntityService,
        CancellationToken cancellationToken = default)
    {
        var items = new List<ClusterReadResultDto>();

        foreach (var cluster in targetClusters.entities)
            items.Add(await ToClusterReadResultDto(cluster, clusterToDestinationMappingEntityService, cancellationToken));

        return new ClusterReadCollectionResultDto
        {
            Total = targetClusters.total,
            Items = items.ToArray()
        };
    }

    public static ClusterReadNotificationDto ToClusterReadNotificationDto(Cluster cluster,
        IReadOnlyCollection<Destination> nodes)
    {
        return new ClusterReadNotificationDto
        {
            Alias = cluster.Alias,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            HealthCheckInterval = cluster.HealthCheckInterval,
            HealthCheckPath = cluster.HealthCheckPath,
            //TODO
            RouteAlias = null,
            Destinations = nodes.Select(DestinationMapper.ToNodeReadResultBase)
                .ToArray(),

        };
    }

    public static ClusterUpdatedNotificationDto ToClusterUpdatedNotificationDto(Cluster cluster)
    {
        return new ClusterUpdatedNotificationDto
        {
            Alias = cluster.Alias,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            HealthCheckInterval = cluster.HealthCheckInterval,
            HealthCheckPath = cluster.HealthCheckPath,
            //TODO
            RouteAlias = null,
            Destinations = null,

        };
    }

    public static ClusterCreatedNotification ToClusterCreatedNotification(Cluster cluster,
        IReadOnlyCollection<Destination> nodes)
    {
        return new ClusterCreatedNotification
        {
            Alias = cluster.Alias,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            HealthCheckInterval = cluster.HealthCheckInterval,
            HealthCheckPath = cluster.HealthCheckPath,
            //TODO
            RouteAlias = null,
            Destinations = nodes.Select(DestinationMapper.ToNodeReadResultBase)
                .ToArray(),
        };
    }

    public static ClusterDeletedNotificationDto ToClusterDeletedNotificationDto(Cluster cluster)
    {
        return new ClusterDeletedNotificationDto
        {
            Alias = cluster.Alias
        };
    }

    public static async Task<ClusterReadCollectionNotificationDto> ToClusterReadCollectionNotificationDto(
        IReadOnlyCollection<Cluster> entites, IDestinationEntityService destinationEntityService,
        IClusterToDestinationMappingEntityService clusterToDestinationMappingEntityService,
        CancellationToken cancellationToken = default)
    {
        var resultItems = new List<ClusterReadNotificationDto>();

        foreach (var entity in entites)
        {
            var nodeIds = (await clusterToDestinationMappingEntityService.GetByEntityLeftIdAsync(entity.Id, PageModel.Full,
                true,
                cancellationToken)).entities.Select(x => x.EntityRightId);

            var nodes = await destinationEntityService.GetCollection(PageModel.Full,
                query => query.Where(x => nodeIds.Contains(x.Id)));

            resultItems.Add(ToClusterReadNotificationDto(entity, nodes.entities));
        }

        return new ClusterReadCollectionNotificationDto
        {
            Total = resultItems.Count,
            Items = resultItems
        };
    }

    private static async Task<ClusterReadResultDto> ToClusterReadResultOutDto(Cluster cluster,
        IClusterToDestinationMappingEntityService clusterToDestinationMappingEntityService,
        CancellationToken cancellationToken = default)
    {
        return new ClusterReadResultDto
        {
            Id = cluster.Id,
            CreatedAt = cluster.CreatedAt,
            UpdatedAt = cluster.UpdatedAt,
            Alias = cluster.Alias,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            //TODO
            // DestiantionsAlias = clusterToDestinationMappingEntityService is not null
            //     ? (await clusterToDestinationMappingEntityService.GetCollection(PageModel.Max,
            //         query => query.Where(_ => _.EntityLeftId == cluster.Id), true, cancellationToken)).entities
            //     .Select(_ => _.EntityLeftId).ToArray()
            //     : null
        };
    }
}