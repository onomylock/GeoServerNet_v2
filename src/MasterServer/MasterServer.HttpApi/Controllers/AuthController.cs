using System.ComponentModel.DataAnnotations;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthRefreshCommand;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaAliasCommand;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaEmailCommand;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignOutCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller/action]")]
public class AuthController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [Authorize(AuthorizationPolicies.Default)]
    public async Task<IActionResult> SignInViaEmail(
        [Required] [FromBody] AuthSignInViaEmailCommand command,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
    
    [HttpPost]
    [Authorize(AuthorizationPolicies.Default)]
    public async Task<IActionResult> SignInViaAlias(
        [Required] [FromBody] AuthSignInViaAliasCommand command,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.AuthorizedExpired)]
    public async Task<IActionResult> Refresh(
        [Required] [FromBody] AuthRefreshCommand command,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.Authorized)]
    public async Task<IActionResult> SignOut(
        [Required] [FromBody] AuthSignOutCommand command,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }    
}