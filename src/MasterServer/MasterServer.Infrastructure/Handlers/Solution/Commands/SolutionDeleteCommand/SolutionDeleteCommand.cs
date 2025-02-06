using MasterServer.Application.Models.Dto.Solution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Solution.Commands.SolutionDeleteCommand;

public class SolutionDeleteCommand : SolutionTargetRequestDto, IRequest<ResponseBase<OkResult>>;