using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Auth;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaEmailCommand;

public class AuthSignInViaEmailCommand : AuthSignInTargetBaseDto
{
    /// <summary>
    ///     Email address
    /// </summary>
    /// <example>some@email.com</example>
    [Required]
    [RegularExpression(RegexExpressions.Email)]
    public string Email { get; set; }
}