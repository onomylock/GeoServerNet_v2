using Shared.Common.Models;

namespace MasterServer.Application.Models.Dto.Auth;

public class AuthValidateJwtResultBaseDto
{
    public ClaimEntry[] Claims { get; set; }
}