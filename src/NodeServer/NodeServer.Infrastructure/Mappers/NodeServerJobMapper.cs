using NodeServer.Application.Models.Dto.NodeServerJob;
using NodeServer.Domain.Entities;

namespace NodeServer.Infrastructure.Mappers;

public static class NodeServerJobMapper
{
    public static NodeServerJobReadResultDto ToNodeServerJobReadResultDto(NodeServerJob entity)
    {
        return new NodeServerJobReadResultDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            JobId = entity.JobId
        };
    }
    
}