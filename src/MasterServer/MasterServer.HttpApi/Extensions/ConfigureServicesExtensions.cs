using System.Text.Json.Serialization;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MasterServer.Application.Models.Options;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Data;
using MasterServer.Infrastructure.Repository;
using MasterServer.Infrastructure.Services.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Npgsql;
using Shared.Application.Data;
using Shared.Common.Behaviours;
using Shared.Common.JsonConverters;
using Shared.Common.Models.Options;
using Shared.Infrastructure.Data;

namespace MasterServer.HttpApi.Extensions;

public static class ConfigureServicesExtensions
{
    public static void InitMasterServiceHttpApi(this IHostApplicationBuilder builder)
    {
        var masterServerHttpApiOptions = builder.Configuration.GetSection(nameof(MasterServerHttpApiOptions))
            .Get<MasterServerHttpApiOptions>();

        builder.Services
            .ConfigureDiOptions(builder.Configuration)
            .ConfigureDiConfigureOptions()
            .ConfigureDiAppDbContext(builder.Environment)
            .AddHttpContextAccessor()
            .AddHttpClient()
            .AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1",
                    TermsOfService = null,
                    Description = $"""
                                   An HTTP API of Master backend server [Branch: {builder.Configuration["GIT_BRANCH"] ?? "Unknown"}, Commit: {builder.Configuration["GIT_REV"] ?? "Unknown"}]

                                   For SignalR hubs, connect via <a href="https://www.npmjs.com/package/@microsoft/signalr">@microsoft/signalr</a> (<a href="https://pastebin.com/raw/7qpSm1C1">Example</a>)
                                   """
                });

                swaggerGenOptions.IncludeXmlComments(Path.Join(AppDomain.CurrentDomain.BaseDirectory,
                    "MasterServer.HttpApi.xml"));

                swaggerGenOptions.EnableAnnotations();
            })
            .ConfigureDiRepositories()
            .ConfigureDiServices()
            .ConfigureDiValidators()
            .ConfigureDiHandlers()
            .ConfigureDiBackgroundServices()
            .ConfigureDiHangfire(builder.Environment)
            .AddCors(corsOptions =>
            {
                corsOptions.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(masterServerHttpApiOptions.CorsAllowedOrigins).AllowAnyMethod().AllowAnyHeader()
                        .AllowCredentials() /*.WithExposedHeaders("Content-Disposition")*/
                        ;
                });
            })
            .ConfigureSignalR(builder.Configuration, builder.Environment)
            .ConfigureHttp();
    }

    private static IServiceCollection ConfigureDiRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IMasterServerRepository<>), typeof(MasterServerRepository<>));

        return serviceCollection;
    }

    private static IServiceCollection ConfigureDiServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDestinationEntityService, DestinationEntityService>();
        serviceCollection.AddScoped<IClusterEntityService, ClusterEntityService>();
        serviceCollection.AddScoped<IClusterToDestinationMappingEntityService, ClusterToDestinationMappingEntityService>();

        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return serviceCollection;
    }

    private static IServiceCollection ConfigureDiValidators(this IServiceCollection serviceCollection)
    {
        var assem = AppDomain.CurrentDomain.GetAssemblies();

        serviceCollection.AddValidatorsFromAssemblies(assem);
        return serviceCollection;
    }

    private static IServiceCollection ConfigureDiHandlers(this IServiceCollection serviceCollection)
    {
        var assem = AppDomain.CurrentDomain.GetAssemblies();

        serviceCollection.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });
        return serviceCollection;
    }

    private static IServiceCollection ConfigureDiBackgroundServices(this IServiceCollection serviceCollection)
    {
        // serviceCollection.AddHostedService<ConsumeScopedBackgroundServicesHostedService>();
        // serviceCollection.AddScoped<IScopedBackgroundService, SomeScopedBackgroundService>();

        return serviceCollection;
    }

    private static IServiceCollection ConfigureDiAppDbContext(
        this IServiceCollection serviceCollection,
        IHostEnvironment env
    )
    {
        serviceCollection.AddScoped<IDbContextTransactionAction, DbContextAction<MasterServerDbContext>>();

        serviceCollection.AddDbContext<MasterServerDbContext>((provider, builder) =>
                DbContextOptionsBuilder(builder,
                    provider.GetRequiredService<IOptions<MasterServerDbContextOptions>>().Value),
            ServiceLifetime.Scoped,
            ServiceLifetime.Singleton
        );
        serviceCollection.AddDbContextFactory<MasterServerDbContext>((provider, builder) =>
            DbContextOptionsBuilder(builder,
                provider.GetRequiredService<IOptions<MasterServerDbContextOptions>>().Value));

        return serviceCollection;

        void DbContextOptionsBuilder(DbContextOptionsBuilder options, MasterServerDbContextOptions appDbContextOptions)
        {
            var connectionString = appDbContextOptions.ConnectionString +
                                   (!env.IsProduction() ? ";Include Error Detail=true" : "");

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();

            options
                .UseNpgsql(dataSource,
                    npgsqlDbContextOptionsBuilder =>
                        npgsqlDbContextOptionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));

            if (!env.IsProduction()) options.EnableSensitiveDataLogging().EnableDetailedErrors();
        }
    }

    private static IServiceCollection ConfigureDiConfigureOptions(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }

    private static IServiceCollection ConfigureDiHangfire(
        this IServiceCollection serviceCollection,
        IHostEnvironment env
    )
    {
        serviceCollection
            .AddHangfire((provider, globalConfiguration) =>
            {
                var hangfireOptions = provider.GetRequiredService<IOptions<HangfireOptions>>().Value;
                var hangfireDbContextOptions = hangfireOptions.DbContextOptions;

                var connectionString = hangfireDbContextOptions.ConnectionString +
                                       (!env.IsProduction() ? ";Include Error Detail=true" : "");

                globalConfiguration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString),
                        new PostgreSqlStorageOptions
                        {
                            QueuePollInterval = TimeSpan.FromSeconds(hangfireOptions.QueuePollIntervalSeconds)
                        });
            });
        //TODO; add for nodes
        // serviceCollection.AddHangfireServer(options =>
        //     options.ServerName = $"{Environment.MachineName}");

        return serviceCollection;
    }

    private static IServiceCollection ConfigureDiOptions(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    )
    {
        serviceCollection.AddOptions();

        var masterServerHttpApiConfigSection = configuration.GetSection(nameof(MasterServerHttpApiOptions));

        serviceCollection.AddOptions<MasterServerHttpApiOptions>().Bind(masterServerHttpApiConfigSection)
            .ValidateDataAnnotations().ValidateOnStart();
        serviceCollection.AddOptions<MasterServerOptions>().Bind(masterServerHttpApiConfigSection)
            .ValidateDataAnnotations().ValidateOnStart();
        serviceCollection.AddOptions<CommonServiceOptions>().Bind(masterServerHttpApiConfigSection)
            .ValidateDataAnnotations().ValidateOnStart();
        serviceCollection.AddOptions<JsonWebTokenOptions>().Bind(configuration.GetSection(nameof(JsonWebTokenOptions)))
            .ValidateDataAnnotations().ValidateOnStart();

        serviceCollection.AddOptions<HangfireOptions>().Bind(configuration.GetSection(nameof(HangfireOptions)))
            .ValidateDataAnnotations().ValidateOnStart();
        //serviceCollection.AddOptions<MinioOptions>().Bind(configuration.GetSection(nameof(MinioOptions))).ValidateDataAnnotations().ValidateOnStart();

        serviceCollection.AddOptions<MasterServerDbContextOptions>()
            .Bind(configuration.GetSection(nameof(MasterServerDbContextOptions))).ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }

    private static IServiceCollection ConfigureSignalR(this IServiceCollection serviceCollection,
        IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        serviceCollection
            .AddSignalR(options => { options.EnableDetailedErrors = true; })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNameCaseInsensitive = false;
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                options.PayloadSerializerOptions.IncludeFields = true;
                options.PayloadSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.PayloadSerializerOptions.Converters.Add(new StringTrimmingJsonConverter());
                //In JS/TS there might be a problem converting string enum into a number, either disable that converter or use https://pastebin.com/raw/uxndBZgZ
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return serviceCollection;
    }

    private static IServiceCollection ConfigureHttp(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddMvc();

        serviceCollection
            .AddControllers()
            .AddControllersAsServices()
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
                jsonOptions.JsonSerializerOptions.IncludeFields = true;
                jsonOptions.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                jsonOptions.JsonSerializerOptions.Converters.Add(new StringTrimmingJsonConverter());
            });

        return serviceCollection;
    }
}