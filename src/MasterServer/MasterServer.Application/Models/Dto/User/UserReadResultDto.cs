using Shared.Common.Models.DTO.Base;

namespace MasterServer.Application.Models.Dto.User;

public class UserReadResultDto : EntityResponseBase
{
    public string Alias { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string PasswordHashed { get; set; }
    public bool Active { get; set; }
    public string Email { get; set; }
    public IEnumerable<Guid> UserGroupIds { get; set; }
}