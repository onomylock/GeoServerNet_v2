using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record JsonWebTokenRevoked : EntityBase
{
    public Guid JsonWebTokenId { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}