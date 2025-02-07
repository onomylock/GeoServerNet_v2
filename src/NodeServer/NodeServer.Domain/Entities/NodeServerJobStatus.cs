using Shared.Domain.Entity.Base;

namespace NodeServer.Domain.Entities;

public record NodeServerJobStatus : EntityBase
{
    public string JobStatus { get; set; }
}