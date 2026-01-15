using Crit.Contracts.RequestModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class CreateCustomFieldTypeToCustomFieldTypeMapper : IMapper<CreateCustomFieldTypeRequest, CustomFieldType>
{
	public CustomFieldType ConvertTo(CreateCustomFieldTypeRequest fromModel)
	{
		return new CustomFieldType()
		{
			Name = fromModel.Name,
			HiddenProjectId = fromModel.HiddenProjectId,
			ProjectId = fromModel.ProjectId
		};
	}
}
