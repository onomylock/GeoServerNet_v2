using MasterServer.Application.Models.Dto.Cluster.Notification;
using MasterServer.Application.Services;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using MasterServer.Infrastructure.Hubs;
using MasterServer.Infrastructure.Mappers;
using Microsoft.AspNetCore.SignalR;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Services;

public sealed class ClusterNotificationService(
    IHubContext<ClusterHub, IClusterHubActions> clusterHub,
    IClusterToNodeMappingEntityService clusterToNodeMappingEntityService,
    INodeEntityService nodeEntityService, IClusterEntityService clusterEntityService
) : IClusterNotificationService
{
    public async Task SendClusterCreatedNotification(Cluster cluster, CancellationToken cancellationToken = default)
    {
        var targetClusterToNodeMappings =
            await clusterToNodeMappingEntityService.GetByEntityRightIdAsync(cluster.Id, PageModel.Max, true, cancellationToken);
        var targetNodes = await nodeEntityService.GetCollection(PageModel.Max,
            query => query.Where(_ => targetClusterToNodeMappings.entities.Any(__ => __.EntityRightId == _.Id)),
            cancellationToken: cancellationToken);
        var result = new ClusterCreatedNotificationDto
        {
            Id = cluster.Id,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            Nodes = targetNodes.entities.Select(NodeMapper.ToNodeReadResultBase).ToList()
        };
        
        await clusterHub.Clients.All.SendClusterCreated(result);
    }

    public async Task SendClusterUpdatedNotification(Cluster cluster, CancellationToken cancellationToken = default)
    {
        var targetClusterToNodeMappings =
            await clusterToNodeMappingEntityService.GetByEntityRightIdAsync(cluster.Id, PageModel.Max, true, cancellationToken);
        var targetNodes = await nodeEntityService.GetCollection(PageModel.Max,
            query => query.Where(_ => targetClusterToNodeMappings.entities.Any(__ => __.EntityRightId == _.Id)),
            cancellationToken: cancellationToken);
        var result = new ClusterUpdatedNotificationDto
        {
            Id = cluster.Id,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            Nodes = targetNodes.entities.Select(NodeMapper.ToNodeReadResultBase).ToList()
        };
        
        await clusterHub.Clients.All.SendClusterUpdated(result);
    }

    public async Task SendClusterConfigInfo(CancellationToken cancellationToken = default)
    {
        var resultItems = new List<ClusterReadNotificationDto>();
        
        var clusters = await clusterEntityService.GetCollection(PageModel.Max, query => query, true, cancellationToken);


        foreach (var cluster in clusters.entities)
        {
            var clusterToNodeMappings =
                await clusterToNodeMappingEntityService.GetByEntityRightIdAsync(cluster.Id, PageModel.Max, true,
                    cancellationToken);
            var nodes = await nodeEntityService.GetCollection(PageModel.Full,
                query => query.Where(_ => clusterToNodeMappings.entities.Any(__ => __.EntityRightId == _.Id)),
                cancellationToken: cancellationToken);
            
            resultItems.AddRange(ClusterMapper.ToClusterReadNotificationDto(cluster, nodes.entities));
        }
        

        var result = new ClusterReadCollectionNotificationDto
        {
            Total = resultItems.Count,
            Items = resultItems
        };

        await clusterHub.Clients.All.SendClusterConfigInfo(result);
        
    }
}