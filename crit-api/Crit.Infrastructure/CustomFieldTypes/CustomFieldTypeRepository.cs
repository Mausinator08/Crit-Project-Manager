using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Models;
using Crit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Crit.Infrastructure.Repositories;

public class CustomFieldTypeRepository : ICustomFieldTypeRepository
{
    private readonly CritDbContext _critDbContext;
    public CustomFieldTypeRepository(CritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
    }

    public async Task<List<CustomFieldType>> GetAllCustomFieldTypes(Guid projectId)
    {
        if (_critDbContext == null)
        {
            throw new InvalidOperationException("CritDbContext is not initialized.");
        }

        if (projectId == Guid.Empty)
        {
            throw new ArgumentException("Project ID cannot be null or empty.", nameof(projectId));
        }

        List<CustomFieldType> customFieldTypes = await _critDbContext.CustomFieldTypes.AsNoTracking().Where(status => status.ProjectId == projectId).ToListAsync();
        return customFieldTypes;
    }

    public async Task<CustomFieldType> GetCustomFieldType(Guid customFieldTypeId)
    {
        if (_critDbContext == null)
        {
            throw new InvalidOperationException("CritDbContext is not initialized.");
        }

        if (customFieldTypeId == Guid.Empty)
        {
            throw new ArgumentException("Custom Field Type ID cannot be null or empty.", nameof(customFieldTypeId));
        }

        List<CustomFieldType> customFieldType = await _critDbContext.CustomFieldTypes.AsNoTracking().Where(customFieldType => customFieldType.Id == customFieldTypeId).ToListAsync();
        if (!customFieldType.Any())
        {
            throw new KeyNotFoundException($"Custom Field Type with ID {customFieldTypeId} not found.");
        }

        return customFieldType.First();
    }

    public async Task<CustomFieldType> CreateCustomFieldType(CustomFieldType customFieldType)
    {
        if (_critDbContext == null)
        {
            throw new InvalidOperationException("CritDbContext is not initialized.");
        }

        if (customFieldType == null)
        {
            throw new ArgumentNullException(nameof(customFieldType), "Custom Field Type cannot be null.");
        }

        EntityEntry<CustomFieldType> createdCustomFieldType = await _critDbContext.CustomFieldTypes.AddAsync(customFieldType);
        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to create Custom Field Type.");
        }

        return createdCustomFieldType.Entity;
    }

    public async Task<CustomFieldType> UpdateCustomFieldType(CustomFieldType customFieldType)
    {
        if (_critDbContext == null)
        {
            throw new InvalidOperationException("CritDbContext is not initialized.");
        }

        if (customFieldType == null)
        {
            throw new ArgumentNullException(nameof(customFieldType), "Custom Field Type cannot be null.");
        }

        EntityEntry<CustomFieldType> updatedCustomFieldType = _critDbContext.CustomFieldTypes.Update(customFieldType);
        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to save Custom Field Type.");
        }

        return updatedCustomFieldType.Entity;
    }

    public async Task DeleteCustomFieldType(Guid customFieldTypeId)
    {
        if (_critDbContext == null)
        {
            throw new InvalidOperationException("CritDbContext is not initialized.");
        }

        if (customFieldTypeId == Guid.Empty)
        {
            throw new ArgumentException("Custom Field Type ID cannot be null or empty.", nameof(customFieldTypeId));
        }

        CustomFieldType customFieldType = await GetCustomFieldType(customFieldTypeId);
        _critDbContext.CustomFieldTypes.Remove(customFieldType);
        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to delete Custom Field Type.");
        }
    }
}
