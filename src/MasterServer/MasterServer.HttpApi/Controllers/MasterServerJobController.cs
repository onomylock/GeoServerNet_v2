using System.ComponentModel.DataAnnotations;
using MasterServer.Infrastructure.Handlers.Job.Commands.JobRefrashCommand;
using MasterServer.Infrastructure.Handlers.Job.Commands.JobStartCommand;
using MasterServer.Infrastructure.Handlers.Job.Commands.JobStopCommand;
using MasterServer.Infrastructure.Handlers.Job.Queries.JobGetResultQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MasterServerJobController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Start([FromBody] [Required] JobStartCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Refresh([FromBody] [Required] JobRefrashCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Stop([FromBody] [Required] JobStopCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> GetResult([FromQuery] [Required] JobGetResultQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }
}