using Shared.Common.Enums;
using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Job : EntityBase
{
    public Guid SolutionId { get; set; }
    public Guid NodeId { get; set; }
    public JobStatus Status { get; set; }
    public string Arguments { get; set; }
}