using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Hubs;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Handlers.Cluster.Notifications.ClusterGetConfigNotification;

public class ClusterGetConfigHandler(
    IHubContext<ClusterHub, IClusterHubActions> clusterHub,
    IClusterEntityService clusterEntityService,
    IClusterToDestinationMappingEntityService clusterToDestinationMappingEntityService,
    IDestinationEntityService destinationEntityService
) : INotificationHandler<ClusterGetConfigNotification>
{
    public async Task Handle(ClusterGetConfigNotification notification, CancellationToken cancellationToken)
    {
        var targetClusters =
            await clusterEntityService.GetCollection(PageModel.Max, query => query, true, cancellationToken);

        var result = await ClusterMapper.ToClusterReadCollectionNotificationDto(targetClusters.entities,
            destinationEntityService, clusterToDestinationMappingEntityService, cancellationToken);

        await clusterHub.Clients.All.SendClusterConfigInfo(result);
    }
}