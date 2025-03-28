using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Common.ValueConverters;
using DbContextOptions = Microsoft.EntityFrameworkCore.DbContextOptions;

namespace MasterServer.Infrastructure.Data;

public class MasterServerDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Node> Nodes { get; set; }
    public DbSet<ClusterToNodeMapping> ClusterToNodeMappings { get; set; }


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetValueConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClusterToNodeMapping>(_ =>
        {
            _.HasIndex(__ => new { __.EntityLeftId, __.EntityRightId }).IsUnique();
        });

        modelBuilder.Entity<Node>();
    }
}