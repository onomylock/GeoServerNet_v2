using Shared.Domain.Entity;
using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record SolutionType : EntityBase
{
    public string Name { get; set; }
    public string ArgumentsMask { get; set; }
}