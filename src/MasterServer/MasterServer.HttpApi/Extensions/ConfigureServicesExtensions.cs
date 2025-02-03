using System.Text.Json.Serialization;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MasterServer.Application.Models.Options;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.AuthenticationHandlers;
using MasterServer.Infrastructure.ConfigureNamedOptions;
using MasterServer.Infrastructure.Data;
using MasterServer.Infrastructure.Repository;
using MasterServer.Infrastructure.Services;
using MasterServer.Infrastructure.Services.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Npgsql;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.AuthenticationHandlers;
using Shared.Common.AuthenticationSchemeOptions;
using Shared.Common.AuthorizationRequirement;
using Shared.Common.AuthorizationRequirementHandlers;
using Shared.Common.Behaviours;
using Shared.Common.ConfigureNamedOptions;
using Shared.Common.ConfigureOptions;
using Shared.Common.Filters;
using Shared.Common.Helpers;
using Shared.Common.JsonConverters;
using Shared.Common.Models;
using Shared.Common.Models.Options;
using Shared.Infrastructure.Data;

namespace MasterServer.HttpApi.Extensions;

public static class ConfigureServicesExtensions
{
    public static void InitMasterServiceHttpApi(this IHostApplicationBuilder builder)
    {
        var masterServerHttpApiOptions = builder.Configuration.GetSection(nameof(MasterServerHttpApiOptions))
            .Get<MasterServerHttpApiOptions>();
        //var minioOptions = builder.Configuration.GetSection(nameof(MinioOptions)).Get<MinioOptions>();

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

                // swaggerGenOptions.AddSignalRSwaggerGen(ssgOptions => ssgOptions.ScanAssemblies([typeof(ChatHub).Assembly]));

                swaggerGenOptions.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Scheme = "Bearer",
                        Name = "Authorization"
                    });

                swaggerGenOptions.OperationFilter<AuthorizeCheckOperationFilter>();

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
            .ConfigureAuthentication()
            .ConfigureAuthorization()
            //.ConfigureSignalR(builder.Configuration, builder.Environment)
            .ConfigureHttp();
        //.AddMinio(_ => _.WithEndpoint(minioOptions.Endpoint).WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey));
    }

    private static IServiceCollection ConfigureDiRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IMasterServerRepository<>), typeof(MasterServerRepository<>));

        return serviceCollection;
    }

    private static IServiceCollection ConfigureDiServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IJobEntityService, JobEntityService>();
        serviceCollection.AddScoped<IJsonWebTokenRevokedEntityService, JsonWebTokenRevokedEntityService>();
        serviceCollection.AddScoped<INodeEntityService, NodeEntityService>();
        serviceCollection.AddScoped<IRefreshTokenEntityService, RefreshTokenEntityService>();
        serviceCollection.AddScoped<ISolutionEntityService, SolutionEntityService>();
        serviceCollection.AddScoped<IUserEntityService, UserEntityService>();
        serviceCollection.AddScoped<IUserGroupEntityService, UserGroupEntityService>();
        serviceCollection.AddScoped<IUserToUserGroupMappingEntityService, UserToUserGroupMappingEntityService>();

        //Those services are a must have for HttpApi/GrpcApi
        serviceCollection.AddScoped<IJsonWebTokenAdvancedService, JsonWebTokenAdvancedService>();
        serviceCollection.AddScoped<IUserAdvancedService, UserAdvancedService>();

        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));


        //serviceCollection.AddSingleton<IMinioService, MinioService>();
        // serviceCollection.AddKeyedScoped<IRedisCacheService, RedisCacheService>(ServiceKeys.Generic, (provider, _) => new RedisCacheService(
        //     provider.GetRequiredService<ILogger<RedisCacheService>>(),
        //     provider.GetRequiredService<IRedisService>(),
        //     provider.GetRequiredService<IOptions<MasterServerHttpApiOptions>>().Value.RedisGenericDatabase
        // ));
        // serviceCollection.AddSingleton<IRedisService, RedisService>(provider => new RedisService(
        //     provider.GetRequiredService<IOptions<RedisOptions>>().Value, provider.GetRequiredService<IHostEnvironment>())
        // );
        // serviceCollection.AddScoped<IWarningService, WarningService>();

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
        serviceCollection.AddSingleton<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticationOptions>();
        serviceCollection
            .AddSingleton<IConfigureOptions<JsonWebTokenAuthenticationSchemeOptions>,
                MasterServerConfigureJwtBearerOptions>(
                provider => new MasterServerConfigureJwtBearerOptions(provider
                    .GetRequiredService<IOptions<JsonWebTokenOptions>>().Value));
        serviceCollection
            .AddSingleton<IConfigureOptions<AccessTokenAuthenticationSchemeOptions>, ConfigureAccessTokenOptions>();

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

    private static IServiceCollection ConfigureAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddAuthentication()
            .AddScheme<DefaultAuthenticationSchemeOptions, DefaultAuthenticationHandler>(AuthenticationSchemes.Default,
                null!)
            .AddScheme<AccessTokenAuthenticationSchemeOptions, AccessTokenAuthenticationHandler>(
                AuthenticationSchemes.AccessToken, null!)
            .AddScheme<JsonWebTokenAuthenticationSchemeOptions, MasterServerJsonWebTokenAuthenticationHandler>(
                AuthenticationSchemes.JsonWebToken, null!)
            .AddScheme<JsonWebTokenAuthenticationSchemeOptions, MasterServerJsonWebTokenExpiredAuthenticationHandler>(
                AuthenticationSchemes.JsonWebTokenExpired, null!);

        return serviceCollection;
    }

    private static IServiceCollection ConfigureAuthorization(this IServiceCollection serviceCollection)
    {
        /*
         * One authorization policy can have multiple requirements (all must succeed - AND operator)
         *
         * And requirements can have multiple handlers (any can succeed, others skip - OR operator)
         *
         * Calling context.Fail() fails entire pipeline!
         */

        serviceCollection
            .AddAuthorization<IServiceProvider>((authorizationOptions, provider) =>
            {
                authorizationOptions.AddPolicy(AuthorizationPolicies.Default,
                    policy =>
                    {
                        policy.Requirements.Add(new DefaultAR());
                        policy.AuthenticationSchemes = DefaultAR.AuthenticationSchemes;
                    });
                authorizationOptions.AddPolicy(AuthorizationPolicies.System,
                    policy =>
                    {
                        policy.Requirements.Add(new SystemAR(provider
                            .GetRequiredService<IOptions<MasterServerHttpApiOptions>>().Value.SystemAccessToken));
                        policy.AuthenticationSchemes = SystemAR.AuthenticationSchemes;
                    });
                authorizationOptions.AddPolicy(AuthorizationPolicies.Authorized,
                    policy =>
                    {
                        policy.Requirements.Add(new AuthorizedAR());
                        policy.AuthenticationSchemes = AuthorizedAR.AuthenticationSchemes;
                    });
                authorizationOptions.AddPolicy(AuthorizationPolicies.AuthorizedExpired,
                    policy =>
                    {
                        policy.Requirements.Add(new AuthorizedExpiredAR());
                        policy.AuthenticationSchemes = AuthorizedExpiredAR.AuthenticationSchemes;
                    });
                authorizationOptions.AddPolicy(AuthorizationPolicies.AuthorizedOrDefault,
                    policy =>
                    {
                        policy.Requirements.Add(new AuthorizedOrDefaultAR());
                        policy.AuthenticationSchemes = AuthorizedOrDefaultAR.AuthenticationSchemes;
                    });
                authorizationOptions.AddPolicy(AuthorizationPolicies.SystemOrAuthorized,
                    policy =>
                    {
                        policy.Requirements.Add(new SystemOrAuthorizedAR(provider
                            .GetRequiredService<IOptions<MasterServerHttpApiOptions>>().Value.SystemAccessToken));
                        policy.AuthenticationSchemes = SystemOrAuthorizedAR.AuthenticationSchemes;
                    });
                authorizationOptions.AddPolicy(AuthorizationPolicies.SystemOrAuthorizedOrDefault,
                    policy =>
                    {
                        policy.Requirements.Add(new SystemOrAuthorizedOrDefaultAR(provider
                            .GetRequiredService<IOptions<MasterServerHttpApiOptions>>().Value.SystemAccessToken));
                        policy.AuthenticationSchemes = SystemOrAuthorizedOrDefaultAR.AuthenticationSchemes;
                    });
            });

        serviceCollection.AddScoped<IAuthorizationHandler, DefaultARH>();
        serviceCollection.AddScoped<IAuthorizationHandler, SystemARH>();
        serviceCollection.AddScoped<IAuthorizationHandler, AuthorizedARH>();
        serviceCollection.AddScoped<IAuthorizationHandler, AuthorizedExpiredARH>();
        serviceCollection.AddScoped<IAuthorizationHandler, AuthorizedOrDefaultARH.Default>();
        serviceCollection.AddScoped<IAuthorizationHandler, AuthorizedOrDefaultARH.Authorized>();
        serviceCollection.AddScoped<IAuthorizationHandler, SystemOrAuthorizedOrDefaultARH.System>();
        serviceCollection.AddScoped<IAuthorizationHandler, SystemOrAuthorizedOrDefaultARH.Authorized>();
        serviceCollection.AddScoped<IAuthorizationHandler, SystemOrAuthorizedOrDefaultARH.Default>();
        serviceCollection.AddScoped<IAuthorizationHandler, SystemOrAuthorizedARH.System>();
        serviceCollection.AddScoped<IAuthorizationHandler, SystemOrAuthorizedARH.Authorized>();

        return serviceCollection;
    }

    //TODO: add SingnalR for publish metrics to client
    // private static IServiceCollection ConfigureSignalR(this IServiceCollection serviceCollection, IConfiguration configuration, IHostEnvironment hostEnvironment)
    // {
    //     var redisOptions = configuration.GetSection(nameof(RedisOptions)).Get<RedisOptions>();
    //
    //     serviceCollection
    //         .AddSignalR(options => { options.EnableDetailedErrors = true; })
    //         .AddStackExchangeRedis(configure =>
    //         {
    //             var configurationOptions = ConfigurationOptions.Parse(redisOptions.ConnectionString);
    //
    //             configure.Configuration = configurationOptions;
    //             configure.Configuration.ChannelPrefix = new RedisChannel(string.Format(RedisChannelPrefix.SignalR, hostEnvironment.EnvironmentName),
    //                 RedisChannel.PatternMode.Literal);
    //         })
    //         .AddJsonProtocol(options =>
    //         {
    //             // options.PayloadSerializerOptions.TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault
    //             //     ? DefaultJsonTypeInfoResolver.RootDefaultInstance()
    //             //     : JsonTypeInfoResolver.Empty,
    //             options.PayloadSerializerOptions.PropertyNameCaseInsensitive = false;
    //             options.PayloadSerializerOptions.PropertyNamingPolicy = null;
    //             options.PayloadSerializerOptions.IncludeFields = true;
    //             options.PayloadSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
    //             options.PayloadSerializerOptions.Converters.Add(new StringTrimmingJsonConverter());
    //             //In JS/TS there might be a problem converting string enum into a number, either disable that converter or use https://pastebin.com/raw/uxndBZgZ
    //             options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    //         });
    //
    //     return serviceCollection;
    // }

    private static IServiceCollection ConfigureHttp(this IServiceCollection serviceCollection)
    {
        // serviceCollection
        //     .Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
        //     {
        //         // options.SuppressModelStateInvalidFilter = true;
        //         apiBehaviorOptions.InvalidModelStateResponseFactory = context =>
        //         {
        //             var errorModelResult = new ErrorModelResult
        //             {
        //                 TraceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
        //             };
        //
        //             foreach (var modelError in context.ModelState.Values.SelectMany(modelStateValue => modelStateValue.Errors))
        //                 errorModelResult.Errors.Add(new ErrorModelResultEntry(ErrorType.ModelState, modelError.ErrorMessage));
        //
        //             return new BadRequestObjectResult(errorModelResult);
        //         };
        //     });

        serviceCollection
            .AddMvc();
        // .ConfigureApiBehaviorOptions(apiBehaviorOptions =>
        // {
        //     // options.SuppressModelStateInvalidFilter = true;
        //     apiBehaviorOptions.InvalidModelStateResponseFactory = context =>
        //     {
        //         var errorModelResult = new ErrorModelResult
        //         {
        //             TraceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
        //         };
        //
        //         foreach (var modelError in context.ModelState.Values.SelectMany(modelStateValue => modelStateValue.Errors))
        //             errorModelResult.Errors.Add(new ErrorModelResultEntry(ErrorType.ModelState, modelError.ErrorMessage));
        //
        //         return new BadRequestObjectResult(errorModelResult);
        //     };
        // });

        serviceCollection
            //.AddControllers(mvcOptions => { mvcOptions.Filters.Add<HttpResponseExceptionFilter>(); })
            .AddControllers(mvcOptions => { mvcOptions.Filters.Add<HttpResponseExceptionFilter>(); })
            .AddControllersAsServices()
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
                jsonOptions.JsonSerializerOptions.IncludeFields = true;
                jsonOptions.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                //In JS/TS there might be a problem converting string enum into a number, either disable that converter or use https://pastebin.com/raw/uxndBZgZ
                jsonOptions.JsonSerializerOptions.Converters.Add(new StringTrimmingJsonConverter());
            });

        return serviceCollection;
    }
}