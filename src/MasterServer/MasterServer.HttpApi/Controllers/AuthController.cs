using System.ComponentModel.DataAnnotations;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthRefreshCommand;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthRefreshWithClaimsCommand;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaAliasCommand;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaEmailCommand;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInWithClaimsCommand;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignOutCommand;
using MasterServer.Infrastructure.Handlers.Auth.Commands.AuthValidateJwtCommand;
using MasterServer.Infrastructure.Handlers.Auth.Queries.AuthIsRevokedQuery;
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
    [Authorize(AuthorizationPolicies.Default)]
    public async Task<IActionResult> SignInWithClaims(
        [Required] [FromBody] AuthSignInWithClaimsCommand command,
        CancellationToken cancellationToken = default)
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
    [Authorize(AuthorizationPolicies.System)]
    public async Task<IActionResult> RefreshWithClaims(
        [Required] [FromBody] AuthRefreshWithClaimsCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.System)]
    public async Task<IActionResult> ValidateJwt(
        [Required] [FromBody] AuthValidateJwtCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.System)]
    public async Task<IActionResult> IsRevoked(
        [Required] [FromQuery] AuthIsRevokedQuery command,
        CancellationToken cancellationToken = default)
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