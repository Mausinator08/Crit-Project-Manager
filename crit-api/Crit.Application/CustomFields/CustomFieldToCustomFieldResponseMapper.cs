using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class CustomFieldToCustomFieldResponseMapper : IMapper<CustomField, CustomFieldResponse>
{
	public CustomFieldResponse ConvertTo(CustomField fromModel)
	{
		return new CustomFieldResponse()
		{
			Id = fromModel.Id,
			CustomFieldTypeId = fromModel.CustomFieldTypeId,
			TaskId = fromModel.TaskId,
			Value = fromModel.Value
		};
	}
}
