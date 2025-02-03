using System.ComponentModel.DataAnnotations;

namespace MasterServer.Application.Models.Dto.UserGroup;

public class UserGroupTargetBaseDto
{
    [Required] public Guid UserGroupId { get; set; }
}