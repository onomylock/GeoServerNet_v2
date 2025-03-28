using MasterServer.Application.Models.Dto.Node;

namespace MasterServer.Application.Models.Dto.Cluster.Notification;

public class ClusterLinkedNodesNotificationDto : ClusterTargetRequestDto
{
    public IReadOnlyCollection<NodeReadResultBase> Nodes { get; set; }
}