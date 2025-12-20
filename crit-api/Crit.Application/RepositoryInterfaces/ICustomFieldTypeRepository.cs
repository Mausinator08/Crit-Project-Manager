using Crit.Domain.Entities;

namespace Crit.Application.RepositoryInterfaces;

public interface ICustomFieldTypeRepository
{
    Task<List<CustomFieldType>> GetAllCustomFieldTypes(Guid projectId);
    Task<CustomFieldType> GetCustomFieldType(Guid customFieldTypeId);
    Task<CustomFieldType> CreateCustomFieldType(CustomFieldType customFieldType);
    Task<CustomFieldType> UpdateCustomFieldType(CustomFieldType customFieldType);
    Task DeleteCustomFieldType(Guid customFieldTypeId);
}