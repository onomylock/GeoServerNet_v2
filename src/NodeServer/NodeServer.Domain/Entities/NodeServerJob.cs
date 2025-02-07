using System.ComponentModel.DataAnnotations.Schema;
using Shared.Domain.Entity.Base;

namespace NodeServer.Domain.Entities;

public record NodeServerJob : EntityBase
{
    public Guid SolutionId { get; set; }
    [Column(TypeName = "jsonb")] public string Arguments { get; set; }
    public string TmpResultPath { get; set; }
    public Guid JobStatusId { get; set; }
}