using MasterServer.Application.Models.Dto.SolutionType;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.SolutionType.Queries.SolutionTypeReadQuery;

public class SolutionTypeReadQuery : SolutionTypeTargetRequestDto, IRequest<ResponseBase<SolutionTypeReadResultDto>>;