using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Auth;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaEmailCommand;

public class AuthSignInViaEmailCommand : AuthSignInRequestBaseDto, IRequest<ResponseBase<AuthSignInResultBaseDto>>
{
    /// <summary>
    ///     Email address
    /// </summary>
    /// <example>some@email.com</example>
    [Required]
    [RegularExpression(RegexExpressions.Email)]
    public string Email { get; set; }
}