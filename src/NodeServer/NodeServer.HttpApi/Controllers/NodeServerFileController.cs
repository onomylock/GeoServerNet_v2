using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NodeServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NodeServerFileController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] [Required] NodeServerFileGetListQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }
}