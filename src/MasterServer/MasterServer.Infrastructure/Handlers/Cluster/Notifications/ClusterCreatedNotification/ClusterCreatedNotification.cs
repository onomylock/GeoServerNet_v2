using MasterServer.Application.Models.Dto.Cluster.Notification;
using MediatR;

namespace MasterServer.Infrastructure.Handlers.Cluster.Notifications.ClusterCreatedNotification;

public class ClusterCreatedNotification : ClusterCreatedNotificationDto, INotification;