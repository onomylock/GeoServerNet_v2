using MasterServer.Application.Models.Dto.SolutionType;
using MasterServer.Domain.Entities;

namespace MasterServer.Infrastructure.Mappers;

public static class SolutionTypeMapper
{
    public static SolutionTypeReadResultDto ToSolutionTypeReadResultDto(SolutionType solutionType)
    {
        return new SolutionTypeReadResultDto
        {
            Alias = solutionType.Alias,
            ArgumentsMask = solutionType.ArgumentsMask,
        };
    }

    public static SolutionTypeReadCollectionResultDto ToSolutionTypeReadCollectionResultDto(
        (int total, IReadOnlyCollection<SolutionType> entities) data)
    {
        var items = new List<SolutionTypeReadResultDto>();

        foreach (var entity in data.entities)
        {
            items.Add(ToSolutionTypeReadResultDto(entity));
        }

        return new SolutionTypeReadCollectionResultDto
        {
            Total = data.total,
            Items = items.ToArray()
        };
    }
}