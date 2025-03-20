namespace MasterServer.Application.Models.Dto.Cluster.Notification;

public class ClusterCreatedNotificationDto : ClusterReadNotificationDto
{
    public Guid UserId { get; set; }
}