using Shared.Domain.Entity;

namespace MasterServer.Domain.Entities;

public record SolutionType : EntityBase
{
    public string Name { get; set; }
    public string ArgumentsMask { get; set; }
}