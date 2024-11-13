using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CritDataAccess.Contexts;

public interface ITenantDbContext
{
    void Configure(Action<DatabaseFacade> databaseAction);
}
