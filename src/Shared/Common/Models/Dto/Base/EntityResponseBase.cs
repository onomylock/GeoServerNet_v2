namespace Shared.Common.Models.DTO.Base;

public class EntityResponseBase
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}