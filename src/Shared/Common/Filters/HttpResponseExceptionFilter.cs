using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Common.Exceptions;

namespace Shared.Common.Filters;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        //TODO: replace to my exceptions 
        // var errorModelResult = new ErrorModelResult
        // {
        //     TraceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
        // };
        //
        // var environment = context.HttpContext.RequestServices.GetService<IWebHostEnvironment>();
        // var logger = context.HttpContext.RequestServices.GetService<ILogger<HttpResponseExceptionFilter>>();
        //
        // switch (context.Exception)
        // {
        //     case LocalizedException localizedException:
        //         logger.LogError(context.Exception, "Handled error!");
        //
        //         errorModelResult.Errors.Add(new ErrorModelResultEntry(ErrorType.Generic, localizedException.AsUi, ErrorEntryType.Message));
        //
        //         if (!environment.IsProduction())
        //         {
        //             errorModelResult.Errors.Add(new ErrorModelResultEntry(ErrorType.Generic, context.Exception.StackTrace, ErrorEntryType.StackTrace));
        //             errorModelResult.Errors.Add(new ErrorModelResultEntry(ErrorType.Generic, context.Exception.Source, ErrorEntryType.Source));
        //             errorModelResult.Errors.Add(new ErrorModelResultEntry(ErrorType.Generic, context.HttpContext.Request.Path, ErrorEntryType.Path));
        //         }
        //
        //         context.Result = new ObjectResult(errorModelResult)
        //         {
        //             StatusCode = StatusCodes.Status500InternalServerError
        //         };
        //
        //         context.ExceptionHandled = true;
        //         break;
        //     case { }:
        //         //This is handled by /Error controller
        //
        //         context.ExceptionHandled = false;
        //         break;
        // }
    }

    public int Order => int.MaxValue - 10;
}