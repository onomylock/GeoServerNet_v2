using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Auth;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthRefreshWithClaimsCommand;

public class AuthRefreshWithClaimsCommand : AuthRefreshRequestBaseDto, IRequest<ResponseBase<AuthSignInResultBaseDto>>
{
    [MinLength(1)] public ClaimEntry[] Claims { get; set; }
}