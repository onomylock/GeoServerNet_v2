using FluentValidation;
using MasterServer.Application.Models.Dto.Solution;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Solution.Queries.SolutionReadCollectionSearchQuery;

public class SolutionReadCollectionSearchHandler(
    IValidator<SolutionReadCollectionSearchQuery> validator,
    ISolutionEntityService solutionEntityService,
    ISolutionTypeEntityService solutionTypeEntityService
) : IRequestHandler<SolutionReadCollectionSearchQuery, ResponseBase<SolutionReadCollectionResultDto>>
{
    public async Task<ResponseBase<SolutionReadCollectionResultDto>> Handle(SolutionReadCollectionSearchQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var dataTerm = request.Term.ToLowerInvariant();

#pragma warning disable CA1862
        var targetSolutions = await solutionEntityService.GetCollection(request.PageModel, query =>
        {
            return query
                .Where(_ => string.IsNullOrEmpty(request.Term)
                            || _.FileName.ToLower().Contains(dataTerm) || dataTerm.Contains(_.FileName.ToLower())
                            || _.BucketName.ToLower().Contains(dataTerm) || dataTerm.Contains(_.BucketName.ToLower())
                            || _.SolutionTypeId.ToString().ToLower().Contains(dataTerm) || dataTerm.Contains(_.SolutionTypeId.ToString().ToLower()));
        }, true, cancellationToken);
#pragma warning restore CA1862

        return new ResponseBase<SolutionReadCollectionResultDto>()
        {
            Data = await SolutionMapper.ToSolutionReadCollectionResultDto(targetSolutions, solutionTypeEntityService,
                cancellationToken)
        };
    }
}