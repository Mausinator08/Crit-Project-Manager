using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface ICustomFieldTypeRepository
{
    Task<List<CustomFieldType>> GetAllCustomFieldTypes(string projectId);
    Task<CustomFieldType> GetCustomFieldType(string customFieldTypeId);
    Task<CustomFieldType> CreateCustomFieldType(CustomFieldType CustomFieldType);
    Task<CustomFieldType> UpdateCustomFieldType(CustomFieldType CustomFieldType);
    Task DeleteCustomFieldType(string CustomFieldTypeId);
}