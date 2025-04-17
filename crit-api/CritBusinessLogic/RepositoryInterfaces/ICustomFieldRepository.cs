using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface ICustomFieldRepository
{
    Task<List<CustomField>> GetAllCustomFields(string taskId);
    Task<CustomField> GetCustomField(string customFieldId);
    Task<CustomField> CreateCustomField(CustomField customField);
    Task<CustomField> UpdateCustomField(CustomField customField);
    Task DeleteCustomField(string customFieldId);
}
