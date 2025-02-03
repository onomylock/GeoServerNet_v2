using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using Shared.Common.Models;
using Shared.Common.ValueConverters;

namespace MasterServer.Infrastructure.Data;

public class MasterServerDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MasterServerDbContext>
{
    public MasterServerDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MasterServerDbContext>();

        var argsDict = new Dictionary<string, string>(10);

        for (var i = 0; i < args.Length - 1; i += 2) argsDict.Add(args[i], args[i + 1]);

        if (!argsDict.TryGetValue("--host", out var host))
            throw new ApplicationException("--host is not defined!");

        if (!argsDict.TryGetValue("--database", out var database))
            throw new ApplicationException("--database is not defined!");

        if (!argsDict.TryGetValue("--port", out var port))
            throw new ApplicationException("--port is not defined!");

        if (!argsDict.TryGetValue("--username", out var username))
            throw new ApplicationException("--username is not defined!");

        if (!argsDict.TryGetValue("--password", out var password))
            throw new ApplicationException("--password is not defined!");

        var appDbContextOptions = new MasterServerDbContextOptions
        {
            ConnectionString =
                $"Host={host};Port={port};Database={database};Username={username};Password={password};Timeout=60;CommandTimeout=60;Pooling=false;"
        };

        var connectionString = appDbContextOptions.ConnectionString + ";Include Error Detail=true";

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        options
            .UseNpgsql(dataSource, npgsqlDbContextOptionsBuilder =>
                {
                    npgsqlDbContextOptionsBuilder
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                }
            );

        options.EnableSensitiveDataLogging().EnableDetailedErrors();

        return new MasterServerDbContext(options.Options);
    }
}