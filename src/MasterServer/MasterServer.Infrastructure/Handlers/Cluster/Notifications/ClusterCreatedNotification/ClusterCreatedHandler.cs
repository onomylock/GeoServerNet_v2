using MasterServer.Infrastructure.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace MasterServer.Infrastructure.Handlers.Cluster.Notifications.ClusterCreatedNotification;

public class ClusterCreatedHandler(IHubContext<ClusterHub, IClusterHubActions> clusterHub)
    : INotificationHandler<ClusterCreatedNotification>
{
    public async Task Handle(ClusterCreatedNotification notification, CancellationToken cancellationToken)
    {
        await clusterHub.Clients.All.SendClusterCreated(notification);
    }
}