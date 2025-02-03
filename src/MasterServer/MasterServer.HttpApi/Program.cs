using MasterServer.HttpApi.Extensions;
using MasterServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Settings.Configuration;

//TODO: This disables requirement of SSL/TLS, useless during development and in secure environments
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

ConfigureExtensions.InitBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

//STAGE: ConfigureServices

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration, new ConfigurationReaderOptions { SectionName = "Serilog" })
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithAssemblyName()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .Enrich.WithMemoryUsage()
        .Enrich.WithProcessId()
        .Enrich.WithProcessName()
        .Enrich.WithThreadId()
        .Enrich.WithThreadName();
});

builder.InitMasterServiceHttpApi();

var app = builder.Build();

//STAGE: Configure

var logger = app.Services.GetRequiredService<ILogger<Program>>();

var appDbContextFactory = app.Services.GetRequiredService<IDbContextFactory<MasterServerDbContext>>();
var appDbContext = await appDbContextFactory.CreateDbContextAsync();

logger.LogInformation("Migrating database...");
await appDbContext.Database.MigrateAsync();

if (!await appDbContext.Database.CanConnectAsync())
    throw new ApplicationException("Failed connecting to database!");

logger.LogInformation("Environment: {0}", app.Environment.EnvironmentName);

ConfigureExtensions.ConfigureMiddlewarePipeline(app);

try
{
    logger.LogInformation("Application setting is finished...");

    app.Run();

    logger.LogInformation("Application stopping...");
}
catch (Exception e)
{
    logger.LogCritical(e, "An unhandled exception occured during bootstrapping!");
}
finally
{
    logger.LogInformation("Flushing logs...");

    Log.CloseAndFlush();
}