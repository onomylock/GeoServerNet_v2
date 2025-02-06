using MasterServer.Application.Models.Dto.Solution;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Mappers;

public static class SolutionMapper
{
    public static async Task<SolutionReadResultDto> ToSolutionReadResultDto(
        Solution solution,
        ISolutionTypeEntityService solutionTypeEntityService,
        CancellationToken cancellationToken = default
    )
    {
        return new SolutionReadResultDto
        {
            Id = solution.Id,
            CreatedAt = solution.CreatedAt,
            UpdatedAt = solution.UpdatedAt,
            FileName = solution.FileName,
            BucketName = solution.BucketName,
            SolutionTypeAlias = (await solutionTypeEntityService.GetByIdAsync(solution.Id, true, cancellationToken))
                .Alias,
        };
    }
    
    

    public static async Task<SolutionReadCollectionResultDto> ToSolutionReadCollectionResultDto(
        (int total, IReadOnlyCollection<Solution> entities) data,
        ISolutionTypeEntityService solutionTypeEntityService,
        CancellationToken cancellationToken = default)
    {
        var items = new List<SolutionReadResultDto>(PageModel.Max.PageSize);

        foreach (var entity in data.entities)
        {
            items.Add(await ToSolutionReadResultDto(entity, solutionTypeEntityService, cancellationToken));
        }

        return new SolutionReadCollectionResultDto
        {
            Total = data.total,
            Items = items
        };
    }
}