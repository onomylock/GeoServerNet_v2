using MasterServer.Infrastructure.Handlers.Destination.Queries.DestinationReadCollectionSearchQuery;
using MasterServer.Infrastructure.Handlers.Node.Commands.NodeCreateCommand;
using MasterServer.Infrastructure.Handlers.Node.Commands.NodeDeleteCommand;
using MasterServer.Infrastructure.Handlers.Node.Commands.NodeUpdateCommand;
using MasterServer.Infrastructure.Handlers.Node.Queries.NodeReadQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DestinationController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Read([FromQuery] DestinationReadQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> ReadCollection([FromQuery] DestinationReadCollectionSearchQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DestinationCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] DestinationUpdateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DestinationDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
}