using System.ComponentModel.DataAnnotations;

namespace Shared.Domain.Entity;

public interface IEntityBase
{
    [Key] public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public abstract record EntityBase : IEntityBase
{
    //https://www.npgsql.org/efcore/modeling/concurrency.html?tabs=data-annotations
    [Timestamp] public uint Version { get; set; }

    public bool IsNew => Id == default;

    [Key] public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}