using MasterServer.Application.Models.Dto.Destination;

namespace MasterServer.Application.Models.Dto.Cluster.Notification;

public class ClusterReadNotificationDto
{
    public string Alias { get; set; }
    public string LoadBalancingPolicy { get; set; }
    public int HealthCheckInterval { get; set; } 
    public string HealthCheckPath { get; set; }
    public string RouteAlias { get; set; }
    public IReadOnlyCollection<DestinationReadResultBase> Destinations { get; set; }
}