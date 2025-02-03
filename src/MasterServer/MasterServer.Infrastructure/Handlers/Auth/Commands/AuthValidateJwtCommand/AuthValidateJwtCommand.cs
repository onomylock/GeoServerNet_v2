using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Auth;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthValidateJwtCommand;

public class AuthValidateJwtCommand : IRequest<ResponseBase<AuthValidateJwtResultBaseDto>>
{
    [Required] [MinLength(1)] public string Token { get; set; }

    [Required] [MinLength(1)] public string AuthenticationScheme { get; set; }
}