using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface ICustomFieldRepository
{
    Task<List<CustomField>> GetAllCustomFields(Guid taskId);
    Task<CustomField> GetCustomField(Guid customFieldId);
    Task<CustomField> CreateCustomField(CustomField customField);
    Task<CustomField> UpdateCustomField(CustomField customField);
    Task DeleteCustomField(Guid customFieldId);
}
