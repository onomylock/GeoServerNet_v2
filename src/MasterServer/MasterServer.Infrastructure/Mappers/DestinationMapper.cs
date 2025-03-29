using MasterServer.Application.Models.Dto.Destination;
using MasterServer.Domain.Entities;

namespace MasterServer.Infrastructure.Mappers;

public static class DestinationMapper
{
    public static DestinationReadResultBase ToNodeReadResultBase(Destination entity)
    {
        return new DestinationReadResultBase
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            //TODO
            Alias = null,
            Host = null,
            Address = null,
        };
    }
}