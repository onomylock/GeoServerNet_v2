using System.ComponentModel.DataAnnotations;

namespace MasterServer.Application.Models.Dto.Auth;

public class AuthSignInRequestBaseDto
{
    /// <summary>
    ///     Password
    /// </summary>
    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    /// <summary>
    ///     Date past which RefreshToken becomes expired - session lifetime
    /// </summary>
    public DateTimeOffset RefreshTokenExpireAt { get; set; }

    /// <summary>
    ///     If active, JsonWebToken and RefreshToken will be set as HTTP Secure cookies, otherwise sent in response
    /// </summary>
    [Required]
    public bool UseCookies { get; set; } = true;
}