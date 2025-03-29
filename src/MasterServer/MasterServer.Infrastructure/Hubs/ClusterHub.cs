using MasterServer.Application.Models.Dto.Cluster.Notification;
using MasterServer.Infrastructure.Handlers.Cluster.Notifications.ClusterGetConfigNotification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Common.Hubs.Base;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Hubs;

public interface IClusterHubActions : IHubBaseAction
{
    Task SendClusterUpdated(ClusterUpdatedNotificationDto notification);
    Task SendClusterCreated(ClusterCreatedNotificationDto notification);
    Task SenClusterDeleted(ClusterDeletedNotificationDto notification);
    Task SendClusterConfigInfo(ClusterReadCollectionNotificationDto notificationDto);
}

[Authorize(AuthorizationPolicies.System)]
public sealed class ClusterHub(
    ILogger<ClusterHub> logger,
    IHostEnvironment hostEnvironment,
    IMediator mediator
) : HubBase<IClusterHubActions>(logger, hostEnvironment)
{
    public override Task OnConnectedAsync()
    {
        mediator.Publish(new ClusterGetConfigNotification());
        return base.OnConnectedAsync();
    }
}