using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using NodeServer.Application.Models.Dto.NodeServerJob;
using NodeServer.Infrastructure.Handlers.Job.Commands.NodeServerJobRefrashCommand;
using NodeServer.Infrastructure.Handlers.NodeServerJob.Commands.NodeServerJobStartCommand;
using NodeServer.Infrastructure.Handlers.NodeServerJob.Commands.NodeServerJobStopCommand;
using NodeServer.Infrastructure.Handlers.NodeServerJob.Queries.NodeServerJobGetResultQuery;
using Shared.Common.Attributes;
using Shared.Common.Filters;
using Shared.Common.Helpers;
using Shared.Common.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace NodeServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NodeServerJobController(IMediator mediator) : ControllerBase
{
    [DisableFormValueModelBinding]
    [RequestSizeLimit(Consts.SolutionMaxFileSize + 4096)]
    [RequestFormLimits(MultipartBodyLengthLimit = Consts.SolutionMaxFileSize)]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    [SwaggerOperationFilter(typeof(MultipartRequestFilter<NodeServerJobStartRequestDto>))]
    public async Task<IActionResult> Start(CancellationToken cancellationToken = default)
    {
        if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            throw new MultipartRequestHelper.RequestIsNotMultipartException();

        var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType));
        var reader = new MultipartReader(boundary, HttpContext.Request.Body);

        //Json
        var multipartSection = await reader.ReadNextSectionAsync(cancellationToken) ??
                               throw new MultipartSectionHelper.MultipartSectionNotFoundException();

        if (!ContentDispositionHeaderValue.TryParse(multipartSection.ContentDisposition,
                out var contentDispositionForm))
            throw new MultipartSectionHelper.MultipartSectionContentDispositionParseFailedException();

        if (!MultipartRequestHelper.HasFormDataContentDisposition(contentDispositionForm))
            throw new MultipartSectionHelper.MultipartSectionContentDispositionFormExpectedException();

        var encoding = multipartSection.GetEncoding();

        if (encoding == null)
            throw new MultipartSectionHelper.MultipartSectionEncodingRetrievalFailedException();

        using var streamReader = new StreamReader(multipartSection.Body, encoding, true, 1024);

        var data = await JsonSerializer.DeserializeAsync<NodeServerJobStartRequestDto>(streamReader.BaseStream,
            cancellationToken: cancellationToken);

        //File
        multipartSection = await reader.ReadNextSectionAsync(cancellationToken) ??
                           throw new MultipartSectionHelper.MultipartSectionNotFoundException();

        if (!ContentDispositionHeaderValue.TryParse(multipartSection.ContentDisposition,
                out var contentDispositionFile))
            throw new MultipartSectionHelper.MultipartSectionContentDispositionParseFailedException();

        if (!MultipartRequestHelper.HasFileContentDisposition(contentDispositionFile))
            throw new MultipartSectionHelper.MultipartSectionContentDispositionFileExpectedException();

        await using var fileStream = multipartSection.Body;
        
        return Ok(await mediator.Send(new NodeServerJobStartCommand
        {
            FileStream = fileStream,
            FileName = WebUtility.HtmlEncode(contentDispositionFile.FileName.Value),
            Metadata = data.Metadata,
            SolutionId = data.SolutionId
        }, cancellationToken));
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