using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignOutCommand;

public class AuthSignOutCommand : IRequest<ResponseBase<OkResult>>
{
    /// <summary>
    ///     If not using cookies, pass RefreshToken here
    /// </summary>
    public string RefreshToken { get; set; }
}