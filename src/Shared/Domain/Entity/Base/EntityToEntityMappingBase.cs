namespace Shared.Domain.Entity.Base;

public interface IEntityToEntityMappingBase
{
    public Guid EntityLeftId { get; set; }
    public Guid EntityRightId { get; set; }
}

public abstract record EntityToEntityMappingBase : EntityBase, IEntityToEntityMappingBase
{
    /*
     * Do not use FK constraints:
     * 1) To save space
     * 2) Let the app logic do cascade operations programmatically
     * 3) Let the app logic do relation checks programmatically
     * 4) Type of entity might not be known outside of service domain (especially in multi-service systems)
     */
    public Guid EntityLeftId { get; set; }
    public Guid EntityRightId { get; set; }
}