using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodeServer.Infrastructure.Handlers.Job.Commands.NodeServerJobRefrashCommand;
using NodeServer.Infrastructure.Handlers.Job.Commands.NodeServerJobStopCommand;
using NodeServer.Infrastructure.Handlers.Job.Queries.NodeServerJobGetResultQuery;
using NodeServer.Infrastructure.Handlers.NodeServerJob.Commands.NodeServerJobStartCommand;
using Shared.Common.Models;

namespace NodeServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NodeServerJobController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Start([FromBody] [Required] NodeServerJobStartCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Refresh([FromBody] [Required] NodeServerJobRefrashCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Stop([FromBody] [Required] NodeServerJobStopCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> GetResult([FromQuery] [Required] NodeServerJobGetResultQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }
}