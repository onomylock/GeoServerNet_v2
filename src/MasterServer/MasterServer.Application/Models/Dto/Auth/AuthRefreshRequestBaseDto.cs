namespace MasterServer.Application.Models.Dto.Auth;

public class AuthRefreshRequestBaseDto
{
    /// <summary>
    ///     If not using cookies, pass RefreshToken here
    /// </summary>
    public string RefreshToken { get; set; }
}