using MasterServer.Application.Models.Dto.Cluster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterDeleteCommand;

public class ClusterDeleteCommand : ClusterTargetRequestDto, IRequest<ResponseBase<OkResult>>;