using System.ComponentModel.DataAnnotations;
using MasterServer.Infrastructure.Handlers.SolutionType.Commands.SolutionTypeCreateCommand;
using MasterServer.Infrastructure.Handlers.SolutionType.Commands.SolutionTypeDeleteCommand;
using MasterServer.Infrastructure.Handlers.SolutionType.Commands.SolutionTypeUpdateCommand;
using MasterServer.Infrastructure.Handlers.SolutionType.Queries.SolutionTypeReadCollectionSearchQuery;
using MasterServer.Infrastructure.Handlers.SolutionType.Queries.SolutionTypeReadQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SolutionTypeController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Create([FromBody] [Required] SolutionTypeCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
    
    [HttpDelete]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Delete([FromBody] [Required] SolutionTypeDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Update([FromBody] [Required] SolutionTypeUpdateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> ReadCollection([FromQuery] [Required] SolutionTypeReadCollectionSearchQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Read([FromQuery] [Required] SolutionTypeReadQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }
}