using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Contexts;
using Crit.Domain.Models;
using Microsoft.EntityFrameworkCore;

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

            List<CustomFieldType> customFieldTypes = await _critDbContext.CustomFieldTypes.AsNoTracking().Where(status => status.ProjectId == projectId).ToListAsync();
            return customFieldTypes;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving custom field types.", ex);
        }
    }

    public async Task<CustomFieldType> GetCustomFieldType(Guid customFieldTypeId)
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving Custom Field Type with ID {customFieldTypeId}.", ex);
        }
    }

    public async Task<CustomFieldType> CreateCustomFieldType(CustomFieldType customFieldType)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (customFieldType == null)
            {
                throw new ArgumentNullException(nameof(customFieldType), "Custom Field Type cannot be null.");
            }

            await _critDbContext.CustomFieldTypes.AddAsync(customFieldType);
            await _critDbContext.SaveChangesAsync();
            return customFieldType;
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating Custom Field Type.", ex);
        }
    }

    public async Task<CustomFieldType> UpdateCustomFieldType(CustomFieldType customFieldType)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (customFieldType == null)
            {
                throw new ArgumentNullException(nameof(customFieldType), "Custom Field Type cannot be null.");
            }

            _critDbContext.CustomFieldTypes.Update(customFieldType);
            await _critDbContext.SaveChangesAsync();
            return customFieldType;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating Custom Field Type.", ex);
        }
    }

    public async Task DeleteCustomFieldType(Guid customFieldTypeId)
    {
        try
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
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting Custom Field Type with ID {customFieldTypeId}.", ex);
        }
    }
}
