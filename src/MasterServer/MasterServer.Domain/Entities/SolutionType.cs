using System.ComponentModel.DataAnnotations.Schema;
using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record SolutionType : EntityBase
{
    public string Alias { get; set; }

    [Column(TypeName = "jsonb")] public string ArgumentsMask { get; set; }
}