using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Models;
using Shared.Common.ValueConverters;
using DbContextOptions = Microsoft.EntityFrameworkCore.DbContextOptions;

namespace MasterServer.Infrastructure.Data;

public class MasterServerDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<JsonWebTokenRevoked> JsonWebTokensRevoked { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<UserToUserGroupMapping> UserToUserGroupMappings { get; set; }
    public DbSet<Node> Nodes { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Solution> Solutions { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetValueConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // var customPasswordHasher = new CustomPasswordHasher();
        // var passwordHash = customPasswordHasher.HashPassword("public_user_password");


        modelBuilder.Entity<User>(_ =>
        {
            _.HasIndex(__ => __.Alias).IsUnique();
            _.HasIndex(__ => __.Email).IsUnique();

            _.HasData(new User
            {
                Alias = "Public",
                Id = Consts.PublicUserId,
                PasswordHashed = "AQAAAAEAACcQAAAAEB865BgMrFifPjLdVzTaX2mj6sYFRcQuqemhEGP4naELoxpdgl5M3I/GFlIyNgD1WA=="
            });

            _.HasData(new User
            {
                Alias = "Root",
                Id = Consts.RootUserId,
                PasswordHashed = "AQAAAAEAACcQAAAAEB865BgMrFifPjLdVzTaX2mj6sYFRcQuqemhEGP4naELoxpdgl5M3I/GFlIyNgD1WA=="
            });
        });

        modelBuilder.Entity<UserGroup>(_ =>
        {
            _.HasIndex(__ => __.Alias).IsUnique();

            _.HasData(new List<UserGroup>
            {
                new()
                {
                    Id = Consts.RootUserGroupId,
                    Alias = "Root"
                },
                new()
                {
                    Id = Consts.ManageUsersUserGroupId,
                    Alias = "ManageUsers"
                },
                new()
                {
                    Id = Consts.ManageAlertLevelsUserGroupId,
                    Alias = "ManageAlertLevels"
                },
                new()
                {
                    Id = Consts.ManageEventsUserGroupId,
                    Alias = "ManageEventsLevels"
                }
            });
        });

        modelBuilder.Entity<UserToUserGroupMapping>(_ =>
        {
            _.HasIndex(__ => new { __.EntityLeftId, __.EntityRightId }).IsUnique();

            _.HasData(new UserToUserGroupMapping
            {
                Id = new Guid("EE549AEA-9436-41F2-9FDD-706439F0DC91"),
                EntityLeftId = Consts.RootUserId,
                EntityRightId = Consts.RootUserGroupId
            });
        });

        modelBuilder.Entity<JsonWebTokenRevoked>(_ => { _.HasIndex(__ => __.JsonWebTokenId).IsUnique(); });

        modelBuilder.Entity<RefreshToken>(_ => { _.HasIndex(__ => __.Token).IsUnique(); });
    }
}