namespace MasterServer.Application.Models.Dto.Auth;

public class AuthSignInResultBaseDto
{
    public Guid UserId { get; set; }
    public string JsonWebToken { get; set; }
    public DateTimeOffset? JsonWebTokenExpiresAt { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
}