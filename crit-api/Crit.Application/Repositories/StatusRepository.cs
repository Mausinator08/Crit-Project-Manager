using Crit.Application.RepositoryInterfaces;
using Crit.Contracts.Models;
using Crit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Crit.Application.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly CritDbContext _critDbContext;
    public StatusRepository(CritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
    }

    public async Task<List<Status>> GetAllStatuses(Guid projectId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (projectId == Guid.Empty)
            {
                throw new ArgumentException("Project ID cannot be null or empty.", nameof(projectId));
            }

            List<Status> statuses = await _critDbContext.Statuses.AsNoTracking().Where(status => status.ProjectId == projectId).ToListAsync();
            return statuses;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving statuses.", ex);
        }
    }

    public async Task<Status> GetStatus(Guid statusId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (statusId == Guid.Empty)
            {
                throw new ArgumentException("Status ID cannot be null or empty.", nameof(statusId));
            }

            List<Status> status = await _critDbContext.Statuses.AsNoTracking().Where(status => status.Id == statusId).ToListAsync();
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
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (status == null)
            {
                throw new ArgumentNullException(nameof(status), "Status cannot be null.");
            }

            await _critDbContext.Statuses.AddAsync(status);
            int savedChanges = await _critDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Failed to create status.");
            }

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
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (status == null)
            {
                throw new ArgumentNullException(nameof(status), "Status cannot be null.");
            }

            _critDbContext.Statuses.Update(status);
            int savedChanges = await _critDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Failed to update status.");
            }

            return status;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating status.", ex);
        }
    }

    public async Task DeleteStatus(Guid statusId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (statusId == Guid.Empty)
            {
                throw new ArgumentException("Status ID cannot be null or empty.", nameof(statusId));
            }

            Status status = await GetStatus(statusId);
            _critDbContext.Statuses.Remove(status);
            int savedChanges = await _critDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Failed to delete status.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting status with ID {statusId}.", ex);
        }
    }
}
