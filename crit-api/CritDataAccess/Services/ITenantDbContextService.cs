using System.Security.Claims;
using CritDataAccess.Contexts;
using CritDTO.Identity;

namespace CritDataAccess.Services;

public interface ITenantDbContextService
{
    Task<bool> DestroyTenantDb(string databaseName);
    Task<TenantDbContext> GetTenantDb(string databaseName);
    Task<TenantDbContext> GetAuthenticatedTenantDb(ClaimsPrincipal user);
    string ConnectionString { get; }
}
