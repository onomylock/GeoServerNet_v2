using MasterServer.Application.Models.Dto.Destination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Node.Commands.NodeDeleteCommand;

public class DestinationDeleteCommand : DestinationTargetRequestDto, IRequest<ResponseBase<OkResult>>;