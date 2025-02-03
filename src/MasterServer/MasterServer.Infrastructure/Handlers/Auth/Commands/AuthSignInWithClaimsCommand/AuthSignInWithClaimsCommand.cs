using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Auth;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInWithClaimsCommand;

public class AuthSignInWithClaimsCommand : AuthSignInRequestBaseDto, IRequest<ResponseBase<AuthSignInResultBaseDto>>
{
    [Required]
    public Guid UserId { get; set; }
    
    [MinLength(1)]
    public ClaimEntry[] Claims { get; set; }
}