using Shared.Common.Models.DTO.Base;

namespace MasterServer.Application.Models.Dto.Cluster;

public class ClusterReadResultDto : EntityResponseBase
{
    public Guid ClusterId { get; set; }
    public string LoadBalancingPolicy { get; set; }
    public Guid[] NodeIds { get; set; }
}