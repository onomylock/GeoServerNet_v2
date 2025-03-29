using MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterCreateCommand;
using MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterDeleteCommand;
using MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterUpdateCommand;
using MasterServer.Infrastructure.Handlers.Cluster.Queries.ClusterReadCollectionSearchQuery;
using MasterServer.Infrastructure.Handlers.Cluster.Queries.ClusterReadQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ClusterController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Read([FromQuery] ClusterReadQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> ReadCollection([FromQuery] ClusterReadCollectionSearchQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClusterCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ClusterUpdateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] ClusterDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
}