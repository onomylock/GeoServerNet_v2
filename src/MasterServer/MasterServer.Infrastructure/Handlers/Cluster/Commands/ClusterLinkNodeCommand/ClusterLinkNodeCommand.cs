using MasterServer.Application.Models.Dto.Cluster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterLinkNodeCommand;

public class ClusterLinkNodeCommand : ClusterTargetRequestDto, IRequest<ResponseBase<OkResult>>
{
    public Guid[] NodeIds { get; set; }
}