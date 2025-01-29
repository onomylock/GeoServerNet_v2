using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClientWebApi.HttpApi.Controllers;

[ApiController]
[Route("[controller/action]")]
public class SolutionController(IMediator mediator) : ControllerBase
{
    
}