using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;

namespace Crit.Application.CustomFieldTypes;

public interface ICustomFieldTypeService
{
	Task<List<CustomFieldTypeResponse>> GetAllCustomFieldTypes(Guid projectId);
	Task<CustomFieldTypeResponse> GetCustomFieldType(Guid customFieldTypeId);
	Task<CustomFieldTypeResponse> CreateCustomFieldType(CreateCustomFieldTypeRequest customFieldType);
	Task<CustomFieldTypeResponse> UpdateCustomFieldType(Guid customFieldTypeId, UpdateCustomFieldTypeRequest customFieldType);
	Task DeleteCustomFieldType(Guid customFieldTypeId);
}
