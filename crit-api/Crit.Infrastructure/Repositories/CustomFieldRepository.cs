using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Contexts;
using Crit.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Crit.Infrastructure.Repositories;

public class CustomFieldRepository : ICustomFieldRepository
{
    private readonly CritDbContext _critDbContext;
    public CustomFieldRepository(CritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
    }

    public async Task<List<CustomField>> GetAllCustomFields(Guid taskId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (taskId == Guid.Empty)
            {
                throw new ArgumentException("Task ID cannot be null or empty.", nameof(taskId));
            }

            List<CustomField> customFields = await _critDbContext.CustomFields.AsNoTracking().Where(status => status.TaskId == taskId).ToListAsync();
            return customFields;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving custom fields.", ex);
        }
    }

    public async Task<CustomField> GetCustomField(Guid customFieldId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (customFieldId == Guid.Empty)
            {
                throw new ArgumentException("Custom Field ID cannot be null or empty.", nameof(customFieldId));
            }

            List<CustomField> customField = await _critDbContext.CustomFields.AsNoTracking().Where(customFieldType => customFieldType.Id == customFieldId).ToListAsync();
            if (!customField.Any())
            {
                throw new KeyNotFoundException($"Custom Field with ID {customFieldId} not found.");
            }

            return customField.First();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving Custom Field with ID {customFieldId}.", ex);
        }
    }

    public async Task<CustomField> CreateCustomField(CustomField customField)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (customField == null)
            {
                throw new ArgumentNullException(nameof(customField), "Custom Field cannot be null.");
            }

            await _critDbContext.CustomFields.AddAsync(customField);
            await _critDbContext.SaveChangesAsync();
            return customField;
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating Custom Field Type.", ex);
        }
    }

    public async Task<CustomField> UpdateCustomField(CustomField customField)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (customField == null)
            {
                throw new ArgumentNullException(nameof(customField), "Custom Field cannot be null.");
            }

            _critDbContext.CustomFields.Update(customField);
            await _critDbContext.SaveChangesAsync();
            return customField;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating Custom Field.", ex);
        }
    }

    public async Task DeleteCustomField(Guid customFieldId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new InvalidOperationException("CritDbContext is not initialized.");
            }

            if (customFieldId == Guid.Empty)
            {
                throw new ArgumentException("Custom Field ID cannot be null or empty.", nameof(customFieldId));
            }

            CustomField customField = await GetCustomField(customFieldId);
            _critDbContext.CustomFields.Remove(customField);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting Custom Field with ID {customFieldId}.", ex);
        }
    }
}
