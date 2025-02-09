using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodeServer.Infrastructure.Handlers.NodeServerSolution.Commands.NodeServerSolutionCreateCommand;
using NodeServer.Infrastructure.Handlers.NodeServerSolution.Commands.NodeServerSolutionDeleteCommand;
using NodeServer.Infrastructure.Handlers.NodeServerSolution.Commands.NodeServerSolutionUpdateCommand;
using NodeServer.Infrastructure.Handlers.NodeServerSolution.Queries.NodeServerSolutionReadQuery;
using Shared.Common.Models;

namespace NodeServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NodeServerSolutionController(IMediator mediator): ControllerBase
{
    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Create([FromBody] [Required] NodeServerSolutionCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
    
    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Create([FromBody] [Required] NodeServerSolutionUpdateCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Read([FromQuery] [Required] NodeServerSolutionReadQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }
    
    [HttpDelete]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Delete([FromBody] [Required] NodeServerSolutionDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
}