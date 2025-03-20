namespace MasterServer.Application.Models.Dto.Cluster.Notification;

public class ClusterUpdatedNotificationDto : ClusterReadNotificationDto
{
    public Guid UserId { get; set; }
}