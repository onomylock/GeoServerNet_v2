using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Models.Dto.Cluster.Notification;

namespace MasterServer.Application.Services;

public interface IClusterNotificationService
{
    Task SendClusterCreatedNotification(ClusterReadNotificationDto notification, CancellationToken cancellationToken = default);

    Task SendClusterDeletedNotification(ClusterDeletedNotificationDto notification,
        CancellationToken cancellationToken = default);
    Task SendClusterUpdatedNotification(ClusterUpdatedNotificationDto notification, CancellationToken cancellationToken = default);

    Task SendClusterLinkedNodesNotification(ClusterLinkedNodesNotificationDto notification,
        CancellationToken cancellationToken = default);
    
    Task SendClusterUnlinkedNodesNotification(ClusterUnlinkedNodesNotificationDto notification,
        CancellationToken cancellationToken = default);
    
    Task SendClusterConfigInfo(CancellationToken cancellationToken = default);
}