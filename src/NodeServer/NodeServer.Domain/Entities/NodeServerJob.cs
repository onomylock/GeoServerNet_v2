using System.ComponentModel.DataAnnotations.Schema;
using Shared.Domain.Entity.Base;
using Shared.Domain.View;

namespace NodeServer.Domain.Entities;

public record NodeServerJob : EntityBase
{
    public Guid SolutionId { get; set; }
    public KeyValueEntry[] Metadata { get; set; }
    public string TmpResultPath { get; set; }
    public Guid JobStatusId { get; set; }
    public Guid JobId { get; set; }
}