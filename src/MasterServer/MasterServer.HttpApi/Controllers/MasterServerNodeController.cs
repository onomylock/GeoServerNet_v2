using System.ComponentModel.DataAnnotations;
using MasterServer.Infrastructure.Handlers.Node.Commands.NodeDeleteCommand;
using MasterServer.Infrastructure.Handlers.Node.Commands.NodeUpdateCommand;
using MasterServer.Infrastructure.Handlers.Node.Queries.NodeReadCollectionSearchQuery;
using MasterServer.Infrastructure.Handlers.Node.Queries.NodeReadQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MasterServerNodeController(
    IMediator mediator
) : ControllerBase
{
    [HttpGet]
    [Authorize(AuthorizationPolicies.Authorized)]
    public async Task<IActionResult> ReadCollectionSearch([Required] [FromQuery] NodeReadCollectionSearchQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.Authorized)]
    public async Task<IActionResult> Read([Required] [FromQuery] NodeReadQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.Authorized)]
    public async Task<IActionResult> Update([Required] [FromBody] NodeUpdateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.Authorized)]
    public async Task<IActionResult> Delete([Required] [FromBody] NodeDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
}