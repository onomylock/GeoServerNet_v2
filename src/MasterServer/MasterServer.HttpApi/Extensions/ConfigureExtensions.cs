using System.Diagnostics;
using System.Reflection;
using Serilog;
using Serilog.Settings.Configuration;

namespace MasterServer.HttpApi.Extensions;

public static class ConfigureExtensions
{
    public static void ConfigureMiddlewarePipeline(WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        if (!app.Environment.IsProduction())
        {
            logger.LogInformation("Add Swagger & SwaggerUI");
            app.UseSwagger().UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            
        }
        else
        {
            app.UseHsts();
        }
        
        app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate =
                    "[{httpContextTraceIdentifier}] {httpContextRequestProtocol} {httpContextRequestMethod} {httpContextRequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("httpContextTraceIdentifier", Activity.Current?.Id ?? httpContext.TraceIdentifier);
                    diagnosticContext.Set("httpContextConnectionId", httpContext.Connection.Id);
                    diagnosticContext.Set("httpContextConnectionRemoteIpAddress", httpContext.Connection.RemoteIpAddress);
                    diagnosticContext.Set("httpContextConnectionRemotePort", httpContext.Connection.RemotePort);
                    diagnosticContext.Set("httpContextRequestHost", httpContext.Request.Host);
                    diagnosticContext.Set("httpContextRequestPath", httpContext.Request.Path);
                    diagnosticContext.Set("httpContextRequestProtocol", httpContext.Request.Protocol);
                    diagnosticContext.Set("httpContextRequestIsHttps", httpContext.Request.IsHttps);
                    diagnosticContext.Set("httpContextRequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("httpContextRequestMethod", httpContext.Request.Method);
                    diagnosticContext.Set("httpContextRequestContentType", httpContext.Request.ContentType);
                    diagnosticContext.Set("httpContextRequestContentLength", httpContext.Request.ContentLength);
                    diagnosticContext.Set("httpContextRequestQueryString", httpContext.Request.QueryString);
                    diagnosticContext.Set("httpContextRequestQuery", httpContext.Request.Query);
                    diagnosticContext.Set("httpContextRequestHeaders", httpContext.Request.Headers);
                    diagnosticContext.Set("httpContextRequestCookies", httpContext.Request.Cookies);
                };
            })
            //.UseExceptionHandler("/Error")
            .UseRouting()
            .UseCors()
            // .UseRequestTimeouts()
            // .UseRateLimiter()
            .UseAuthentication()
            .UseAuthorization()
            .UseWebSockets()
            .UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapControllers();

                // endpointRouteBuilder.MapHub<ChatHub>("/hubs/chat");
            });
    }
    
    public static void InitBootstrapLogger()
    {
        var configurationRoot = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, false)
            .AddJsonFile("appsettings.Development.json", true, false)
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configurationRoot, new ConfigurationReaderOptions { SectionName = "Serilog" })
            .Enrich.FromLogContext()
            .Enrich.WithAssemblyName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithMemoryUsage()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .CreateBootstrapLogger();
    }
}