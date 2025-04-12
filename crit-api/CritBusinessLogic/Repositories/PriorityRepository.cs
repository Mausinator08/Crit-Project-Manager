using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class PriorityRepository : IPriorityRepository
{
    private TenantDbContext? _tenantDbContext = null;
    public PriorityRepository(ITenantDbContextService tenantDbContextService, IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            Task<TenantDbContext> tenantDbContextTask = tenantDbContextService.GetAuthenticatedTenantDb(httpContextAccessor.HttpContext.User);
            tenantDbContextTask.Wait();
            _tenantDbContext = tenantDbContextTask.Result;
        }
    }

    public async Task<List<Priority>> GetAllPriorities(string projectId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentException("Project ID cannot be null or empty.", nameof(projectId));
            }

            List<Priority> priorities = await _tenantDbContext.Priorities.AsNoTracking().Where(priority => priority.ProjectId == projectId).ToListAsync();
            return priorities;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving priorities.", ex);
        }
    }

    public async Task<Priority> GetPriority(string priorityId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(priorityId))
            {
                throw new ArgumentException("Priority ID cannot be null or empty.", nameof(priorityId));
            }

            IQueryable<Priority> priority = _tenantDbContext.Priorities.AsNoTracking().Where(priority => priority.Id == priorityId);
            if (!priority.Any())
            {
                throw new KeyNotFoundException($"Priority with ID {priorityId} not found.");
            }

            return priority.First();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving priority with ID {priorityId}.", ex);
        }
    }

    public async Task<Priority> CreatePriority(Priority priority)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (priority == null)
            {
                throw new ArgumentNullException(nameof(priority), "Priority cannot be null.");
            }

            await _tenantDbContext.Priorities.AddAsync(priority);
            await _tenantDbContext.SaveChangesAsync();
            return priority;
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating priority.", ex);
        }
    }

    public async Task<Priority> UpdatePriority(Priority priority)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (priority == null)
            {
                throw new ArgumentNullException(nameof(priority), "Priority cannot be null.");
            }

            _tenantDbContext.Priorities.Update(priority);
            await _tenantDbContext.SaveChangesAsync();
            return priority;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating priority.", ex);
        }
    }

    public async Task DeletePriority(string priorityId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(priorityId))
            {
                throw new ArgumentException("Priority ID cannot be null or empty.", nameof(priorityId));
            }

            Priority priority = await GetPriority(priorityId);
            _tenantDbContext.Priorities.Remove(priority);
            await _tenantDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting priority with ID {priorityId}.", ex);
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
