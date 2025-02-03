using System.ComponentModel.DataAnnotations;
using MasterServer.Infrastructure.Handlers.UserGroup.Queries.UserGroupReadCollectionSearchQuery;
using MasterServer.Infrastructure.Handlers.UserGroup.Queries.UserGroupReadQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserGroupController(
    IMediator mediator
) : ControllerBase
{
    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    //[ProducesResponseType(typeof(UserGroupReadOutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Read(
        [Required] [FromQuery] UserGroupReadQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    //[ProducesResponseType(typeof(UserGroupReadCollectionOutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReadCollectionSearch(
        [Required] [FromQuery] UserGroupReadCollectionSearchQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }
}