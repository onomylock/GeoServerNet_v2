using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Cluster;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterCreateCommand;

public class ClusterCreateCommand : IRequest<ResponseBase<ClusterReadResultDto>>
{
    [Required] public string Alias { get; set; }
    [Required] public string LoadBalancingPolicy { get; set; }
    [Required] public int HealthCheckInterval { get; set; } 
    [Required] public string HealthCheckPath { get; set; }
    [Required] public string RouteAlias { get; set; }
    [Required] public IEnumerable<string> Destinations { get; set; }
}