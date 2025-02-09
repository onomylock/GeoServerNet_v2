using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Common.Models;

namespace Shared.Common.Hubs.Base;

public interface IHubBase
{
    public ErrorModelResult ValidateModel(object data);
    public Task ThrowException(Exception e, ErrorType errorType = ErrorType.Generic, bool abortConnection = false);
}

public interface IHubBaseAction
{
    public Task ReceiveError(ErrorModelResult errorModelResult);
    public Task ReceiveErrorModelResult(ErrorModelResult errorModelResult);
}

public abstract class HubBase<T>(ILogger<HubBase<T>> logger, IHostEnvironment hostEnvironment) : Hub<T>, IHubBase
    where T : class, IHubBaseAction
{
    private readonly ILogger _logger = logger;

    public ErrorModelResult ValidateModel(object data)
    {
        var context = new ValidationContext(data);
        var validationResults = new List<ValidationResult>();

        if (Validator.TryValidateObject(data, context, validationResults, true))
            return null;

        var errorModelResult = new ErrorModelResult
        {
            Errors = []
        };

        foreach (var validationResult in validationResults)
            errorModelResult.Errors.Add(new ErrorModelResultEntry(ErrorType.ModelState, validationResult.ErrorMessage));

        return errorModelResult;
    }

    public async Task ThrowException(Exception e, ErrorType errorType = ErrorType.Generic, bool abortConnection = false)
    {
        _logger.LogError(e, "[{typeNameOf}:{functionNameOf}]", nameof(HubBase<T>), nameof(ThrowException));

        var exceptionHandlerFeature = Context.Features.Get<IExceptionHandlerFeature>()!;
        var httpContextFeature = Context.Features.Get<IHttpContextFeature>()!;

        var errorModelResult = new ErrorModelResult();

        if (hostEnvironment.IsProduction())
        {
            errorModelResult.Errors.Add(new ErrorModelResultEntry(errorType, Localize.Keys.Error.HandledExceptionContactSystemAdministrator, ErrorEntryType.Message));

            await ThrowError(errorModelResult);

            return;
        }

        errorModelResult.Errors.Add(new ErrorModelResultEntry(errorType, exceptionHandlerFeature.Error.Message, ErrorEntryType.Message));
        errorModelResult.Errors.Add(new ErrorModelResultEntry(errorType, exceptionHandlerFeature.Error.StackTrace, ErrorEntryType.StackTrace));
        errorModelResult.Errors.Add(new ErrorModelResultEntry(errorType, exceptionHandlerFeature.Error.Source, ErrorEntryType.Source));
        errorModelResult.Errors.Add(new ErrorModelResultEntry(errorType, exceptionHandlerFeature.Path, ErrorEntryType.Path));

        await ThrowError(errorModelResult, abortConnection);
    }

    private async Task ThrowError(ErrorModelResult errorModelResult, bool abortConnection = false)
    {
        var httpContextFeature = Context.Features.Get<IHttpContextFeature>()!;

        errorModelResult.TraceId = Activity.Current?.Id ?? httpContextFeature.HttpContext!.TraceIdentifier;

        await Clients.Caller.ReceiveError(errorModelResult);

        if (abortConnection)
            Context.Abort();
    }
}