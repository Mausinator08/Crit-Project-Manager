using Crit.Application.Mappers;
using Crit.Application.RepositoryInterfaces;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.CustomFieldTypes;

public class CustomFieldTypeService : ICustomFieldTypeService
{
	private readonly ICustomFieldTypeRepository _customFieldTypeRepository;
	private readonly IMapperService _mapperService;

	public CustomFieldTypeService(ICustomFieldTypeRepository customFieldTypeRepository, IMapperService mapperService)
	{
		_customFieldTypeRepository = customFieldTypeRepository;
		_mapperService = mapperService;
	}

	public async Task<List<CustomFieldTypeResponse>> GetAllCustomFieldTypes(Guid projectId)
	{
		List<CustomFieldType> customFieldTypes = await _customFieldTypeRepository.GetAllCustomFieldTypes(projectId);

		return _mapperService.ConvertListTo<CustomFieldType, CustomFieldTypeResponse>(customFieldTypes);
	}

	public async Task<CustomFieldTypeResponse> GetCustomFieldType(Guid customFieldTypeId)
	{
		CustomFieldType customFieldType = await _customFieldTypeRepository.GetCustomFieldType(customFieldTypeId);

		return _mapperService.ConvertTo<CustomFieldType, CustomFieldTypeResponse>(customFieldType);
	}
	public async Task<CustomFieldTypeResponse> CreateCustomFieldType(CreateCustomFieldTypeRequest customFieldType)
	{
		CustomFieldType createdCustomFieldType = await _customFieldTypeRepository.CreateCustomFieldType(_mapperService.ConvertTo<CreateCustomFieldTypeRequest, CustomFieldType>(customFieldType));

		return _mapperService.ConvertTo<CustomFieldType, CustomFieldTypeResponse>(createdCustomFieldType);
	}

	public async Task<CustomFieldTypeResponse> UpdateCustomFieldType(Guid customFieldTypeId, UpdateCustomFieldTypeRequest customFieldType)
	{
		CustomFieldType customFieldTypeToUpdate = await _customFieldTypeRepository.GetCustomFieldType(customFieldTypeId);

		customFieldTypeToUpdate.Name = customFieldType.Name ?? customFieldTypeToUpdate.Name;
		customFieldTypeToUpdate.HiddenProjectId = customFieldType.HiddenProjectId ?? customFieldTypeToUpdate.HiddenProjectId;
		customFieldTypeToUpdate.ProjectId = customFieldType.ProjectId ?? customFieldTypeToUpdate.ProjectId;

		CustomFieldType updatedCustomFieldType = await _customFieldTypeRepository.UpdateCustomFieldType(customFieldTypeToUpdate);

		return _mapperService.ConvertTo<CustomFieldType, CustomFieldTypeResponse>(updatedCustomFieldType);
	}

	public async Task DeleteCustomFieldType(Guid customFieldTypeId)
	{
		await _customFieldTypeRepository.DeleteCustomFieldType(customFieldTypeId);
	}
}
