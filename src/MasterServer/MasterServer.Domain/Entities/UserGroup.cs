using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record UserGroup : EntityBase
{
    public string Alias { get; set; }
}