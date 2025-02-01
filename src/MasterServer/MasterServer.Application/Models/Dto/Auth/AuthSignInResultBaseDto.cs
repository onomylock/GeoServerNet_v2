namespace MasterServer.Application.Models.Dto.Auth;

public class AuthSignInResultBaseDto
{
    public AuthSignInResultBaseDto()
    {
    }

    public AuthSignInResultBaseDto(Guid userId, string jsonWebToken, DateTimeOffset? jsonWebTokenExpiresAt,
        string refreshToken, DateTimeOffset? refreshTokenExpiresAt)
    {
        UserId = userId;
        JsonWebToken = jsonWebToken;
        JsonWebTokenExpiresAt = jsonWebTokenExpiresAt;
        RefreshToken = refreshToken;
        RefreshTokenExpiresAt = refreshTokenExpiresAt;
    }

    public Guid UserId { get; set; }
    public string JsonWebToken { get; set; }
    public DateTimeOffset? JsonWebTokenExpiresAt { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
}