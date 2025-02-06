using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using MasterServer.Application.Models.Dto.Solution;
using MasterServer.Infrastructure.Handlers.Solution.Commands.SolutionCreateCommand;
using MasterServer.Infrastructure.Handlers.Solution.Commands.SolutionDeleteCommand;
using MasterServer.Infrastructure.Handlers.Solution.Queries.SolutionReadCollectionSearchQuery;
using MasterServer.Infrastructure.Handlers.Solution.Queries.SolutionReadQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Shared.Common.Attributes;
using Shared.Common.Filters;
using Shared.Common.Helpers;
using Shared.Common.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace MasterServer.HttpApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SolutionController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Read([FromQuery] [Required] SolutionReadQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> ReadCollection([FromQuery] [Required] SolutionReadCollectionSearchQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    [DisableFormValueModelBinding]
    [RequestSizeLimit(Consts.SolutionMaxFileSize + 4096)]
    [RequestFormLimits(MultipartBodyLengthLimit = Consts.SolutionMaxFileSize)]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    [SwaggerOperationFilter(typeof(MultipartRequestFilter<SolutionCreateRequestDto>))]
    public async Task<IActionResult> Create(
        CancellationToken cancellationToken = default)
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

        var data = await JsonSerializer.DeserializeAsync<SolutionCreateRequestDto>(streamReader.BaseStream,
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

        return Ok(await mediator.Send(new SolutionCreateCommand
        {
            FileStream = fileStream,
            FileName = WebUtility.HtmlEncode(contentDispositionFile.FileName.Value),
            SolutionTypeAlias = data.SolutionTypeAlias
        }, cancellationToken));
    }

    [HttpDelete]
    [Authorize(AuthorizationPolicies.SystemOrAuthorized)]
    public async Task<IActionResult> Delete([FromBody] [Required] SolutionDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
}