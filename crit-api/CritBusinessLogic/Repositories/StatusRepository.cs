using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class StatusRepository : IStatusRepository
{
    private TenantDbContext? _tenantDbContext = null;
    public StatusRepository(ITenantDbContextService tenantDbContextService, IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            Task<TenantDbContext> tenantDbContextTask = tenantDbContextService.GetAuthenticatedTenantDb(httpContextAccessor.HttpContext.User);
            tenantDbContextTask.Wait();
            _tenantDbContext = tenantDbContextTask.Result;
        }
    }

    public async Task<List<Status>> GetAllStatuses(string projectId)
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

            List<Status> statuses = await _tenantDbContext.Statuses.AsNoTracking().Where(status => status.ProjectId == projectId).ToListAsync();
            return statuses;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving statuses.", ex);
        }
    }

    public async Task<Status> GetStatus(string statusId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(statusId))
            {
                throw new ArgumentException("Status ID cannot be null or empty.", nameof(statusId));
            }

            IQueryable<Status> status = _tenantDbContext.Statuses.AsNoTracking().Where(status => status.Id == statusId);
            if (!status.Any())
            {
                throw new KeyNotFoundException($"Status with ID {statusId} not found.");
            }

            return status.First();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving status with ID {statusId}.", ex);
        }
    }

    public async Task<Status> CreateStatus(Status status)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (status == null)
            {
                throw new ArgumentNullException(nameof(status), "Status cannot be null.");
            }

            await _tenantDbContext.Statuses.AddAsync(status);
            await _tenantDbContext.SaveChangesAsync();
            return status;
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating status.", ex);
        }
    }

    public async Task<Status> UpdateStatus(Status status)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (status == null)
            {
                throw new ArgumentNullException(nameof(status), "Status cannot be null.");
            }

            _tenantDbContext.Statuses.Update(status);
            await _tenantDbContext.SaveChangesAsync();
            return status;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating status.", ex);
        }
    }

    public async Task DeleteStatus(string statusId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(statusId))
            {
                throw new ArgumentException("Status ID cannot be null or empty.", nameof(statusId));
            }

            Status status = await GetStatus(statusId);
            _tenantDbContext.Statuses.Remove(status);
            await _tenantDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting status with ID {statusId}.", ex);
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
