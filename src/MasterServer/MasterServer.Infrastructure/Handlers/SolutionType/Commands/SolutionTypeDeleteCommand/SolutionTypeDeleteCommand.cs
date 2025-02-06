using MasterServer.Application.Models.Dto.SolutionType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.SolutionType.Commands.SolutionTypeDeleteCommand;

public class SolutionTypeDeleteCommand : SolutionTypeTargetRequestDto, IRequest<ResponseBase<OkResult>>;