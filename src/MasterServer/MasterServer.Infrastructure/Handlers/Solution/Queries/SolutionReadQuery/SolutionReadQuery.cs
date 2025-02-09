using MasterServer.Application.Models.Dto.Solution;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Solution.Queries.SolutionReadQuery;

public class SolutionReadQuery : SolutionTargetRequestDto, IRequest<ResponseBase<SolutionReadResultDto>>;