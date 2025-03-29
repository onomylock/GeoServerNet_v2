namespace MasterServer.Application.Models.Dto.Cluster.Notification;

public class ClusterLinkedDestinationNotificationDto : ClusterTargetRequestDto
{
    public IReadOnlyCollection<string> Destinations { get; set; }
}