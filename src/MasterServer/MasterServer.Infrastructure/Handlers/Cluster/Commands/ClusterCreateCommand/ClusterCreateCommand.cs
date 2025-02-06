using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Cluster;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterCreateCommand;

public class ClusterCreateCommand : IRequest<ResponseBase<ClusterReadResultDto>> 
{
    [Required] public string LoadBalancingPolicy { get; set; }
    [Required] public Guid UserId { get; set; }
    public Guid[] NodeIds { get; set; }
}