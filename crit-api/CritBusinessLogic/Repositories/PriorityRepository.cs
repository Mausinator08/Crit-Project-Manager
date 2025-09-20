using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class PriorityRepository : IPriorityRepository
{
    private readonly CritDbContext _critDbContext;
    public PriorityRepository(CritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
    }

    public async Task<List<Priority>> GetAllPriorities(Guid projectId)
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

            List<Priority> priorities = await _critDbContext.Priorities.AsNoTracking().Where(priority => priority.ProjectId == projectId).ToListAsync();
            return priorities;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving priorities.", ex);
        }
    }

    public async Task<Priority> GetPriority(Guid priorityId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (priorityId == Guid.Empty)
            {
                throw new ArgumentException("Priority ID cannot be null or empty.", nameof(priorityId));
            }

            List<Priority> priority = await _critDbContext.Priorities.AsNoTracking().Where(priority => priority.Id == priorityId).ToListAsync();
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
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (priority == null)
            {
                throw new ArgumentNullException(nameof(priority), "Priority cannot be null.");
            }

            await _critDbContext.Priorities.AddAsync(priority);
            await _critDbContext.SaveChangesAsync();
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
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (priority == null)
            {
                throw new ArgumentNullException(nameof(priority), "Priority cannot be null.");
            }

            _critDbContext.Priorities.Update(priority);
            await _critDbContext.SaveChangesAsync();
            return priority;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating priority.", ex);
        }
    }

    public async Task DeletePriority(Guid priorityId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (priorityId == Guid.Empty)
            {
                throw new ArgumentException("Priority ID cannot be null or empty.", nameof(priorityId));
            }

            Priority priority = await GetPriority(priorityId);
            _critDbContext.Priorities.Remove(priority);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting priority with ID {priorityId}.", ex);
        }
    }
}
