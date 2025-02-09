using Shared.Common.Models.DTO.Base;

namespace MasterServer.Application.Models.Dto.Cluster;

public class ClusterReadResultDto : EntityResponseBase
{
    public string LoadBalancingPolicy { get; set; }
    public Guid UserId { get; set; }
    public Guid[] NodeIds { get; set; }
}