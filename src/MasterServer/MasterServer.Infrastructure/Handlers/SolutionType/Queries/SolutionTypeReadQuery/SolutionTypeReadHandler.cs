using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.SolutionType;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.SolutionType.Queries.SolutionTypeReadQuery;

public class SolutionTypeReadHandler(
    IValidator<SolutionTypeReadQuery> validator,
    ISolutionTypeEntityService solutionTypeEntityService
) : IRequestHandler<SolutionTypeReadQuery, ResponseBase<SolutionTypeReadResultDto>>
{
    public async Task<ResponseBase<SolutionTypeReadResultDto>> Handle(SolutionTypeReadQuery request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var targetSolutionType =
            await solutionTypeEntityService.GetByIdAsync(request.SolutionTypeId, true, cancellationToken) ??
            throw new SolutionTypeNotFoundException();

        return new ResponseBase<SolutionTypeReadResultDto>
        {
            Data = SolutionTypeMapper.ToSolutionTypeReadResultDto(targetSolutionType)
        };
    }
}