using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Cluster;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterUpdateCommand;

public class ClusterUpdateCommand : ClusterTargetRequestDto, IRequest<ResponseBase<ClusterReadResultDto>>
{
    [Required] public string LoadBalancingPolicy  { get; set; }
}