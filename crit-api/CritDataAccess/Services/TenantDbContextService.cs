using CritDataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace CritDataAccess.Services;

public class TenantDbContextService : ITenantDbContextService
{
    private readonly string _connectionString;

    public string ConnectionString
    {
        get => _connectionString;
    }

    public TenantDbContextService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> DestroyTenantDb(string databaseName)
    {
        MongoClient client = new MongoClient(_connectionString);
        if (client.GetDatabase(databaseName) != null)
        {
            await client.DropDatabaseAsync(databaseName);

            return true;
        }

        return false;
    }

    public async Task<ITenantDbContext> GetTenantDb(string databaseName)
    {
        TenantDbContext tenantDbContext = new TenantDbContext(new DbContextOptionsBuilder().UseMongoDB(_connectionString, databaseName).Options);

        return tenantDbContext;
    }
}
