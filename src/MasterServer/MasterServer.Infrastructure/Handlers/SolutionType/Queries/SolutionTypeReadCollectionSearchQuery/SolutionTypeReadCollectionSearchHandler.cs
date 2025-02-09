using FluentValidation;
using MasterServer.Application.Models.Dto.SolutionType;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.SolutionType.Queries.SolutionTypeReadCollectionSearchQuery;

public class SolutionTypeReadCollectionSearchHandler(
    IValidator<SolutionTypeReadCollectionSearchQuery> validator,
    ISolutionTypeEntityService solutionTypeEntityService
) : IRequestHandler<SolutionTypeReadCollectionSearchQuery, ResponseBase<SolutionTypeReadCollectionResultDto>>
{
    public async Task<ResponseBase<SolutionTypeReadCollectionResultDto>> Handle(
        SolutionTypeReadCollectionSearchQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var dataTerm = request.Term.ToLowerInvariant();

#pragma warning disable CA1862
        var targetSolutionTypes = await solutionTypeEntityService.GetCollection(request.PageModel, query =>
        {
            return query
                .Where(_ => string.IsNullOrEmpty(request.Term)
                            || _.Alias.ToLower().Contains(dataTerm) || dataTerm.Contains(_.Alias.ToLower())
                            || _.ArgumentsMask.ToLower().Contains(dataTerm) ||
                            dataTerm.Contains(_.ArgumentsMask.ToLower()));
        }, true, cancellationToken);
#pragma warning restore CA1862

        return new ResponseBase<SolutionTypeReadCollectionResultDto>
        {
            Data = SolutionTypeMapper.ToSolutionTypeReadCollectionResultDto(targetSolutionTypes)
        };
    }
}