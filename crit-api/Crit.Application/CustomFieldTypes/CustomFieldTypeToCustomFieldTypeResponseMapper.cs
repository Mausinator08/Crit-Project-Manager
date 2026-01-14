using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class CustomFieldTypeToCustomFieldTypeResponseMapper : IMapper<CustomFieldType, CustomFieldTypeResponse>
{
	private readonly IMapperService _mapperService;

	public CustomFieldTypeToCustomFieldTypeResponseMapper(IMapperService mapperService)
	{
		_mapperService = mapperService;
	}

	public CustomFieldTypeResponse ConvertTo(CustomFieldType fromModel)
	{
		return new CustomFieldTypeResponse()
		{
			Id = fromModel.Id,
			CustomFields = _mapperService.ConvertListTo<CustomField, CustomFieldResponse>(fromModel.CustomFields),
			HiddenProjectId = fromModel.HiddenProjectId,
			Name = fromModel.Name,
			ProjectId = fromModel.ProjectId
		};
	}
}
