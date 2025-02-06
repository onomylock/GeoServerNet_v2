using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.Solution;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Solution.Queries.SolutionReadQuery;

public class SolutionReadHandler(
    IValidator<SolutionReadQuery> validator,
    ISolutionEntityService solutionEntityService,
    ISolutionTypeEntityService solutionTypeEntityService
) : IRequestHandler<SolutionReadQuery, ResponseBase<SolutionReadResultDto>>
{
    public async Task<ResponseBase<SolutionReadResultDto>> Handle(SolutionReadQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var targetSolutionDto = await solutionEntityService.GetByIdAsync(request.SolutionId, true, cancellationToken) ??
                                throw new SolutionNotFoundException();


        return new ResponseBase<SolutionReadResultDto>()
        {
            Data = await SolutionMapper.ToSolutionReadResultDto(targetSolutionDto, solutionTypeEntityService, cancellationToken)
        };

    }
}