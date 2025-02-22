using MasterServer.Application.Models.Dto.Node;

namespace MasterServer.Application.Models.Dto.Cluster.Notification;

public class ClusterReadNotificationDto
{
    public Guid Id { get; set; }
    public string LoadBalancingPolicy { get; set; }
    public IReadOnlyCollection<NodeReadResultBase> Nodes { get; set; }
}