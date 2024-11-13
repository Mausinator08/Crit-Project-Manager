using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CritDataAccess.Contexts;

public class TenantDbContext : DbContext, ITenantDbContext
{
    public TenantDbContext(DbContextOptions options) : base(options)
    {
        ConfigureDefault();
    }

    protected TenantDbContext()
    {
        ConfigureDefault();
    }

    public void Configure(Action<DatabaseFacade> databaseAction)
    {
        databaseAction.Invoke(Database);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureDefault()
    {
        Configure(db =>
        {
            db.EnsureCreated();
        });
    }
}
