using CritDataAccess.Contexts;

namespace CritDataAccess.Services;

public interface ITenantDbContextService
{
    Task<bool> DestroyTenantDb(string databaseName);
    Task<ITenantDbContext> GetTenantDb(string databaseName);
    string ConnectionString { get; }
}
