using System.ComponentModel.DataAnnotations;

namespace MasterServer.Application.Models.Dto.User;

public class UserTargetBaseDto
{
    [Required] public Guid UserId { get; set; }
}