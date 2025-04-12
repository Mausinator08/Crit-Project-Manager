using System.Security.Authentication;
using System.Security.Claims;
using CritDataAccess.Contexts;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace CritDataAccess.Services;

public class TenantDbContextService : ITenantDbContextService
{
    private readonly string _connectionString;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICritDbContext _critDbContext;

    public string ConnectionString
    {
        get => _connectionString;
    }

    public TenantDbContextService(string connectionString, UserManager<ApplicationUser> userManager, ICritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
        _connectionString = connectionString;
        _userManager = userManager;
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

    public async Task<TenantDbContext> GetTenantDb(string databaseName)
    {
        DbContextOptionsBuilder<TenantDbContext> dbContextOptions = new DbContextOptionsBuilder<TenantDbContext>()
            .UseMongoDB(_connectionString, databaseName)
            .EnableSensitiveDataLogging();
        TenantDbContext tenantDbContext = new TenantDbContext(dbContextOptions.Options);

        return tenantDbContext;
    }

    public async Task<TenantDbContext> GetAuthenticatedTenantDb(ClaimsPrincipal user)
    {
        try
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            ApplicationUser? applicationUser = await _userManager.GetUserAsync(user);

            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser));
            }

            IQueryable<Organization> organizationsMemberQuery = _critDbContext.Organizations.AsNoTracking().Where(o => o.MemberUserIds.Contains(applicationUser.Id));
            IQueryable<Organization> organizationsAdminQuery = _critDbContext.Organizations.AsNoTracking().Where(o => o.AdminUserIds.Contains(applicationUser.Id));

            Organization? organization = null;

            if (organizationsMemberQuery.Any())
            {
                organization = organizationsMemberQuery.First();
            }
            else if (organizationsAdminQuery.Any())
            {
                organization = organizationsAdminQuery.First();
            }
            else
            {
                throw new AuthenticationException("User is not a member of any organization.");
            }

            TenantDbContext? tenantDbContext = await GetTenantDb(organization.DatabaseName);

            return tenantDbContext;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
