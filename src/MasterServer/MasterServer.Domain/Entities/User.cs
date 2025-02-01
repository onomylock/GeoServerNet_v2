using Shared.Domain.Entity;
using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record User : EntityBase
{
    public string Alias { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string PasswordHashed { get; set; }
    public bool Active { get; set; }
    public string Email { get; set; }
}