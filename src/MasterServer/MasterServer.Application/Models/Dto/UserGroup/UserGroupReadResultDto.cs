namespace MasterServer.Application.Models.Dto.UserGroup;

public class UserGroupReadResultDto
{
    public string Alias { get; set; }
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}