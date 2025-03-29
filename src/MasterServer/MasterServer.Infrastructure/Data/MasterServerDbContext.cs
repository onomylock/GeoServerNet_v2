using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Common.ValueConverters;
using DbContextOptions = Microsoft.EntityFrameworkCore.DbContextOptions;

namespace MasterServer.Infrastructure.Data;

public class MasterServerDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Destination> Nodes { get; set; }
    public DbSet<ClusterToDestinationMapping> ClusterToNodeMappings { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Cluster> Clusters { get; set; }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetValueConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClusterToDestinationMapping>(_ =>
        {
            _.HasIndex(__ => new { __.EntityLeftId, __.EntityRightId }).IsUnique();
        });

        modelBuilder.Entity<Cluster>()
            .HasIndex(x => x.Alias);
        
        modelBuilder.Entity<Destination>()
            .HasIndex(x => x.Alias);

        modelBuilder.Entity<Route>()
            .HasIndex(x => x.Alias);
    }
}