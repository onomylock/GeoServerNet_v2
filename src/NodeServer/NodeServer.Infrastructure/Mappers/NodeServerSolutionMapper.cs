using NodeServer.Application.Models.Dto.NodeServerSolution;
using NodeServer.Domain.Entities;

namespace NodeServer.Infrastructure.Mappers;

public static class NodeServerSolutionMapper
{
    public static NodeServerSolutionReadResultDto ToNodeServerSolutionReadResultDto(NodeServerSolution entity)
    {
        return new NodeServerSolutionReadResultDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            MasterServerSolutionId = entity.MasterServerSolutionId,
            DirectoryPath = entity.DirectoryPath,
            FileExePath = entity.FileExePath,
            DirectoryResultsPath = entity.DirectoryResultsPath,
            ArgumentsMask = entity.ArgumentsMask
        };
    }
}