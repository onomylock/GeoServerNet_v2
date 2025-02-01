using Shared.Domain.Entity;
using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Solution : EntityBase
{
    public string Name { get; set; }
    public string BucketName { get; set; }
    public Guid SolutionTypeId { get; set; }
}