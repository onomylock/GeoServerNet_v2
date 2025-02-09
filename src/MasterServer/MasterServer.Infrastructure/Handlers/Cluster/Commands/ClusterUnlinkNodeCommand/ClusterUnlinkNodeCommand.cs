using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Cluster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterUnlinkNodeCommand;

public class ClusterUnlinkNodeCommand : ClusterTargetRequestDto, IRequest<ResponseBase<OkResult>>
{
    [Required] public Guid[] NodeIds { get; set; }
}