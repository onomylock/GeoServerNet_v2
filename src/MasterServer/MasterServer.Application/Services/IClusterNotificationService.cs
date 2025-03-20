using MasterServer.Domain.Entities;

namespace MasterServer.Application.Services;

public interface IClusterNotificationService
{
    Task SendClusterCreatedNotification(Cluster cluster, CancellationToken cancellationToken = default);
    Task SendClusterUpdatedNotification(Cluster cluster, CancellationToken cancellationToken = default);
    Task SendClusterConfigInfo(CancellationToken cancellationToken = default);
}