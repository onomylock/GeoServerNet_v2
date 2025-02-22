using MasterServer.Application.Models.Dto.Node;
using MasterServer.Domain.Entities;

namespace MasterServer.Infrastructure.Mappers;

public static class NodeMapper
{
    public static NodeReadResultBase ToNodeReadResultBase(Node entity)
    {
        return new NodeReadResultBase
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Address = entity.Address
        };
    }
}