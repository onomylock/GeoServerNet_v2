using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Destination : EntityBase
{
    public string Alias { get; set; }
    public string Host { get; set; }
    public string Address { get; set; }
}