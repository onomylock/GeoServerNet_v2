using System.ComponentModel.DataAnnotations;
using MasterServer.Domain.Entities;
using MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterCreateCommand;
using MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterDeleteCommand;
using MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterLinkNodeCommand;
using MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterUnlinkNodeCommand;
using MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterUpdateCommand;
using MasterServer.Infrastructure.Handlers.Cluster.Queries.ClusterReadCollectionSearchQuery;
using MasterServer.Infrastructure.Handlers.Cluster.Queries.ClusterReadQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MasterServerClusterController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Read([FromQuery] [Required] ClusterReadQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }
    
    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> ReadCollection([FromQuery] [Required] ClusterReadCollectionSearchQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Create([FromBody] [Required] ClusterCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
    
    [HttpDelete]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Delete([FromBody] [Required] ClusterDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
    
    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Update([FromBody] [Required] ClusterUpdateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> LinkNode([FromBody] [Required] ClusterLinkNodeCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
    
    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> UnlinkNode([FromBody] [Required] ClusterUnlinkNodeCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

}