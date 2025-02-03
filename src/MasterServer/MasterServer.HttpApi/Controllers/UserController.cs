using System.ComponentModel.DataAnnotations;
using MasterServer.Infrastructure.Handlers.User.Commands.UserCreateCommand;
using MasterServer.Infrastructure.Handlers.User.Commands.UserDeleteCommand;
using MasterServer.Infrastructure.Handlers.User.Commands.UserUpdateCommand;
using MasterServer.Infrastructure.Handlers.User.Queries.UserReadByAliasQuery;
using MasterServer.Infrastructure.Handlers.User.Queries.UserReadByEmailQuery;
using MasterServer.Infrastructure.Handlers.User.Queries.UserReadCollectionSearchQuery;
using MasterServer.Infrastructure.Handlers.User.Queries.UserReadQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    //[ProducesResponseType(typeof(UserReadOutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(
        [Required] [FromBody] UserCreateCommand command,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    //[ProducesResponseType(typeof(UserReadOutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Read(
        [Required] [FromQuery] UserReadQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.System)]
    //[ProducesResponseType(typeof(UserReadOutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReadByAlias(
        [Required] [FromQuery] UserReadByAliasQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.System)]
    //[ProducesResponseType(typeof(UserReadOutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReadByEmail(
        [Required] [FromQuery] UserReadByEmailQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    //[ProducesResponseType(typeof(UserReadCollectionOutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReadCollectionSearch(
        [Required] [FromQuery] UserReadCollectionSearchQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpPut]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    //[ProducesResponseType(typeof(UserReadOutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        [Required] [FromBody] UserUpdateCommand command,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    //[ProducesResponseType(typeof(OkOutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(
        [Required] [FromQuery] UserDeleteCommand command,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
}