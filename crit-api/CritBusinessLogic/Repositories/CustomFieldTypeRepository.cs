using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class CustomFieldTypeRepository : ICustomFieldTypeRepository
{
    private TenantDbContext? _tenantDbContext = null;
    public CustomFieldTypeRepository(ITenantDbContextService tenantDbContextService, IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            Task<TenantDbContext> tenantDbContextTask = tenantDbContextService.GetAuthenticatedTenantDb(httpContextAccessor.HttpContext.User);
            tenantDbContextTask.Wait();
            _tenantDbContext = tenantDbContextTask.Result;
        }
    }

    public async Task<List<CustomFieldType>> GetAllCustomFieldTypes(string projectId)
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

            List<CustomFieldType> customFieldTypes = await _tenantDbContext.CustomFieldTypes.AsNoTracking().Where(status => status.ProjectId == projectId).ToListAsync();
            return customFieldTypes;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving custom field types.", ex);
        }
    }

    public async Task<CustomFieldType> GetCustomFieldType(string customFieldTypeId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(customFieldTypeId))
            {
                throw new ArgumentException("Custom Field Type ID cannot be null or empty.", nameof(customFieldTypeId));
            }

            IQueryable<CustomFieldType> customFieldType = _tenantDbContext.CustomFieldTypes.AsNoTracking().Where(customFieldType => customFieldType.Id == customFieldTypeId);
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
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (customFieldType == null)
            {
                throw new ArgumentNullException(nameof(customFieldType), "Custom Field Type cannot be null.");
            }

            await _tenantDbContext.CustomFieldTypes.AddAsync(customFieldType);
            await _tenantDbContext.SaveChangesAsync();
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
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (customFieldType == null)
            {
                throw new ArgumentNullException(nameof(customFieldType), "Custom Field Type cannot be null.");
            }

            _tenantDbContext.CustomFieldTypes.Update(customFieldType);
            await _tenantDbContext.SaveChangesAsync();
            return customFieldType;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating Custom Field Type.", ex);
        }
    }

    public async Task DeleteCustomFieldType(string customFieldTypeId)
    {
        try
        {
            if (_tenantDbContext == null)
            {
                throw new InvalidOperationException("TenantDbContext is not initialized.");
            }

            if (string.IsNullOrEmpty(customFieldTypeId))
            {
                throw new ArgumentException("Custom Field Type ID cannot be null or empty.", nameof(customFieldTypeId));
            }

            CustomFieldType customFieldType = await GetCustomFieldType(customFieldTypeId);
            _tenantDbContext.CustomFieldTypes.Remove(customFieldType);
            await _tenantDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting Custom Field Type with ID {customFieldTypeId}.", ex);
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
