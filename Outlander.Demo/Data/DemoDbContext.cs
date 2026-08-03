using Microsoft.EntityFrameworkCore;

namespace Outlander.Demo.Data;

public sealed class DemoDbContext : DbContext
{
    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
    {
    }

    public DbSet<ServerEntity> Servers => Set<ServerEntity>();
}