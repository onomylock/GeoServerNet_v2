using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Auth;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaAliasCommand;

public class AuthSignInViaAliasCommand : AuthSignInRequestBaseDto, IRequest<ResponseBase<AuthSignInResultBaseDto>>
{
    [Required] [MinLength(1)] public string Alias { get; set; }
}