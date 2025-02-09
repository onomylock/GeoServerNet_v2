using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Solution : EntityBase
{
    public string FileName { get; set; }
    public string BucketName { get; set; }
    public Guid SolutionTypeId { get; set; }
}