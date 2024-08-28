using Microsoft.EntityFrameworkCore;

namespace CritDataAccess.Context;

public class CritSQLContext : DbContext, ICritSQLContext
{
    public CritSQLContext(DbContextOptions options) : base(options)
    {

    }

    protected CritSQLContext()
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
