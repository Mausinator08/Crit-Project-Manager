using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class OrganizationRepository : IDisposable, IAsyncDisposable, IOrganizationRepository
{
    private readonly CritDbContext _critDbContext;
    private TenantDbContext? _tenantDbContext = null;
    private readonly ITenantDbContextService _tenantDbContextService;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectsRepository _projectsRepository;

    public OrganizationRepository(CritDbContext critDbContext, ITenantDbContextService tenantDbContextService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IProjectsRepository projectsRepository)
    {
        _critDbContext = critDbContext;
        _tenantDbContextService = tenantDbContextService;
        _httpContextAccessor = httpContextAccessor;
        if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            if (_critDbContext.Organizations.Any())
            {
                Task<TenantDbContext> tenantDbContextTask = tenantDbContextService.GetAuthenticatedTenantDb(_httpContextAccessor.HttpContext.User);
                tenantDbContextTask.Wait();
                _tenantDbContext = tenantDbContextTask.Result;
            }
        }

        _userRepository = userRepository;
        _projectsRepository = projectsRepository;
    }

    public async Task<Organization> CreateFirstOrganization(Organization organization)
    {
        try
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization), "Organization cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(organization.Name))
            {
                throw new ArgumentException("Organization name cannot be empty.", nameof(organization.Name));
            }

            if (await _critDbContext.Organizations.AnyAsync(o => o.Name == organization.Name))
            {
                throw new InvalidOperationException($"An organization with the name '{organization.Name}' already exists.");
            }

            _critDbContext.Organizations.Add(organization);
            await _critDbContext.SaveChangesAsync();

            if (_httpContextAccessor.HttpContext == null)
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            _tenantDbContext = await _tenantDbContextService.GetAuthenticatedTenantDb(_httpContextAccessor.HttpContext.User);

            return organization;
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException || ex is ArgumentNullException || ex is ArgumentException || ex is UnauthorizedAccessException)
            {
                throw;
            }

            throw new Exception("Error creating the first organization.", ex);
        }
    }

    public async Task<List<Organization>> GetAllOrganizations()
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new NullReferenceException("TenantDbContext is null.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();

            if (organization != null && applicationUser != null)
            {
                IQueryable<Organization> organizationsQuery = _critDbContext.Organizations.Where(o =>
                o.Id == organization.Id &&
                o.AffiliatedUserIds.Contains(applicationUser.Id));

                if (organizationsQuery.Any())
                {
                    return await organizationsQuery.ToListAsync();
                }
            }
            else if (organization == null)
            {
                throw new UnauthorizedAccessException("User is not a member of any organization.");
            }

            return new List<Organization>();
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedAccessException || ex is NullReferenceException)
            {
                throw;
            }

            throw new Exception("Error retrieving all organizations.", ex);
        }
    }

    public async Task<Organization?> GetOrganization(string organizationId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new NullReferenceException("TenantDbContext is null.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();

            if (applicationUser != null)
            {
                IQueryable<Organization> organizationsQuery = _critDbContext.Organizations.Where(o =>
                o.Id == organizationId &&
                o.AffiliatedUserIds.Contains(applicationUser.Id));

                if (organizationsQuery.Any())
                {
                    return await organizationsQuery.FirstAsync();
                }
            }
            else
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            return null;
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedAccessException || ex is NullReferenceException)
            {
                throw;
            }

            throw new Exception($"Error retrieving organization with ID {organizationId}.", ex);
        }
    }

    public async Task<Organization> CreateOrganization(Organization organization)
    {
        try
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization), "Organization cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(organization.Name))
            {
                throw new ArgumentException("Organization name cannot be empty.", nameof(organization.Name));
            }

            if (await _critDbContext.Organizations.AnyAsync(o => o.Name == organization.Name))
            {
                throw new InvalidOperationException($"An organization with the name '{organization.Name}' already exists.");
            }

            IQueryable<Organization> organizationsQuery = _critDbContext.Organizations.Where(o =>
            o.AdminUserIds.Where(a => organization.AdminUserIds.Contains(a)).Any() ||
            o.MemberUserIds.Where(m => organization.MemberUserIds.Contains(m)).Any());

            if (organizationsQuery.Any())
            {
                throw new InvalidOperationException($"Admin and/or member user for organization {organization.Id} is already in another oganization.");
            }

            _critDbContext.Organizations.Add(organization);
            await _critDbContext.SaveChangesAsync();

            return organization;
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException || ex is ArgumentNullException || ex is ArgumentException)
            {
                throw;
            }

            throw new Exception("Error creating the organization.", ex);
        }
    }

    public async System.Threading.Tasks.Task UpdateOrganization(Organization organization)
    {
        try
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization), "Organization cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(organization.Name))
            {
                throw new ArgumentException("Organization name cannot be empty.", nameof(organization.Name));
            }

            IQueryable<Organization> organizationsQuery = _critDbContext.Organizations.Where(o =>
            o.Id == organization.Id &&
            o.AdminUserIds.Where(a => organization.AdminUserIds.Contains(a)).Any() ||
            o.MemberUserIds.Where(m => organization.MemberUserIds.Contains(m)).Any());

            if (organizationsQuery.Any())
            {
                throw new InvalidOperationException($"Admin and/or member user for organization {organization.Id} is already in another oganization.");
            }

            _critDbContext.Organizations.Update(organization);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException || ex is ArgumentNullException || ex is ArgumentException)
            {
                throw;
            }

            throw new Exception("Error updating the organization.", ex);
        }
    }

    public async System.Threading.Tasks.Task DeleteOrganization(string organizationId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new NullReferenceException("CritDbContext is null.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();

            if (applicationUser == null)
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            IQueryable<Organization> organizationsQuery = _critDbContext.Organizations.Where(o => o.Id == organizationId && o.AdminUserIds.Contains(applicationUser.Id));

            if (!organizationsQuery.Any())
            {
                throw new InvalidOperationException($"User with ID {applicationUser.Id} is not an administrator of organization with ID {organizationId}.");
            }

            Organization organization = await organizationsQuery.FirstAsync();

            if (organization == null)
            {
                throw new InvalidOperationException($"Organization with ID {organizationId} does not exist.");
            }

            if (_tenantDbContext == null)
            {
                throw new NullReferenceException("TenantDbContext is null.");
            }

            IQueryable<Project> projectsQuery = _tenantDbContext.Projects.Where(p => p.OwningOrganizationId == organizationId);

            if (projectsQuery.Any())
            {
                throw new InvalidOperationException($"Cannot delete organization {organizationId} because it owns projects.");
            }

            _critDbContext.Organizations.Remove(organization);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException || ex is UnauthorizedAccessException || ex is NullReferenceException)
            {
                throw;
            }

            throw new Exception($"Error deleting organization with ID {organizationId}.", ex);
        }
    }

    public void Dispose()
    {
        if (_tenantDbContext != null)
        {
            _tenantDbContext?.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_tenantDbContext != null)
        {
            await _tenantDbContext.DisposeAsync();
        }
    }
}
