using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface ICustomFieldTypeRepository
{
    Task<List<CustomFieldType>> GetAllCustomFieldTypes(string projectId);
    Task<CustomFieldType> GetCustomFieldType(string customFieldTypeId);
    Task<CustomFieldType> CreateCustomFieldType(CustomFieldType customFieldType);
    Task<CustomFieldType> UpdateCustomFieldType(CustomFieldType customFieldType);
    Task DeleteCustomFieldType(string customFieldTypeId);
}