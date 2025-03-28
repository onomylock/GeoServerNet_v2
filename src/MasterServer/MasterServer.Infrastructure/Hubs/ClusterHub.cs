using MasterServer.Application.Models.Dto.Cluster.Notification;
using MasterServer.Application.Services;
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
    Task SendClusterConfigInfo(ClusterReadCollectionNotificationDto notificationDto);
}

[Authorize(AuthorizationPolicies.System)]
public sealed class ClusterHub(
    ILogger<ClusterHub> logger,
    IHostEnvironment hostEnvironment,
    IClusterNotificationService clusterNotificationService
) : HubBase<IClusterHubActions>(logger, hostEnvironment)
{
    public override Task OnConnectedAsync()
    {
        clusterNotificationService.SendClusterConfigInfo();
        return base.OnConnectedAsync();
    }
}