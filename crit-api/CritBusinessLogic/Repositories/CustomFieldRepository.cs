using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class CustomFieldRepository : ICustomFieldRepository
{
    private TenantDbContext? _tenantDbContext = null;
    public CustomFieldRepository(ITenantDbContextService tenantDbContextService, IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            Task<TenantDbContext> tenantDbContextTask = tenantDbContextService.GetAuthenticatedTenantDb(httpContextAccessor.HttpContext.User);
            tenantDbContextTask.Wait();
            _tenantDbContext = tenantDbContextTask.Result;
        }
    }

    public async Task<List<CustomField>> GetAllCustomFields(string taskId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(taskId))
            {
                throw new ArgumentException("Task ID cannot be null or empty.", nameof(taskId));
            }

            List<CustomField> customFields = await _tenantDbContext.CustomFields.AsNoTracking().Where(status => status.TaskId == taskId).ToListAsync();
            return customFields;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving custom fields.", ex);
        }
    }

    public async Task<CustomField> GetCustomField(string customFieldId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(customFieldId))
            {
                throw new ArgumentException("Custom Field ID cannot be null or empty.", nameof(customFieldId));
            }

            IQueryable<CustomField> customField = _tenantDbContext.CustomFields.AsNoTracking().Where(customFieldType => customFieldType.Id == customFieldId);
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
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (customField == null)
            {
                throw new ArgumentNullException(nameof(customField), "Custom Field cannot be null.");
            }

            await _tenantDbContext.CustomFields.AddAsync(customField);
            await _tenantDbContext.SaveChangesAsync();
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
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (customField == null)
            {
                throw new ArgumentNullException(nameof(customField), "Custom Field cannot be null.");
            }

            _tenantDbContext.CustomFields.Update(customField);
            await _tenantDbContext.SaveChangesAsync();
            return customField;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating Custom Field.", ex);
        }
    }

    public async Task DeleteCustomField(string customFieldId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(customFieldId))
            {
                throw new ArgumentException("Custom Field ID cannot be null or empty.", nameof(customFieldId));
            }

            CustomField customField = await GetCustomField(customFieldId);
            _tenantDbContext.CustomFields.Remove(customField);
            await _tenantDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting Custom Field with ID {customFieldId}.", ex);
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
