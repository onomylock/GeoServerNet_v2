using Microsoft.EntityFrameworkCore;

namespace Shared.Application.Data;

public interface IDbContextAction<out TDbContext> where TDbContext : DbContext
{
    TDbContext DbContext { get; }
}