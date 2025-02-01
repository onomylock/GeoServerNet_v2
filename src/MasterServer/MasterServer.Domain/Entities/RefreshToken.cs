using Shared.Domain.Entity;
using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record RefreshToken : EntityBase
{
    public string Token { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public Guid UserId { get; set; }
}