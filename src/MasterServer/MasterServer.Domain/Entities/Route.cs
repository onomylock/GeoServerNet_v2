using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Route : EntityBase
{
    public string Alias { get; set; }
    public string MatchPath { get; set; }
    public string[] Methods { get; set; }
    public Guid ClusterId { get; set; }
}