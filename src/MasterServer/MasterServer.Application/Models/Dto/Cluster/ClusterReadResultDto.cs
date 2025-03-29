using Shared.Common.Models.DTO.Base;

namespace MasterServer.Application.Models.Dto.Cluster;

public class ClusterReadResultDto : EntityResponseBase
{
    public string Alias { get; set; }
    public string LoadBalancingPolicy { get; set; }
    public int HealthCheckInterval { get; set; } 
    public string HealthCheckPath { get; set; }
    public string RouteAlias { get; set; }
    public string[] DestiantionsAlias { get; set; }
}