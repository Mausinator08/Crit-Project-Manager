using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class ProjectToProjectResponseMapper : IMapper<Project, ProjectResponse>
{
	private readonly IMapperService _mapperService;

	public ProjectToProjectResponseMapper(IMapperService mapperService)
	{
		_mapperService = mapperService;
	}

	public ProjectResponse ConvertTo(Project fromModel)
	{
		return new ProjectResponse()
		{
			Id = fromModel.Id,
			Comments = _mapperService.ConvertListTo<Comment, CommentResponse>(fromModel.Comments),
			CustomFieldTypes = _mapperService.ConvertListTo<CustomFieldType, CustomFieldTypeResponse>(fromModel.CustomFieldTypes),
			HiddenCustomFieldTypes = _mapperService.ConvertListTo<CustomFieldType, CustomFieldTypeResponse>(fromModel.HiddenCustomFieldTypes),
			Name = fromModel.Name,
			Description = fromModel.Description,
			OwningOrganizationId = fromModel.OwningOrganizationId,
			Priorities = _mapperService.ConvertListTo<Priority, PriorityResponse>(fromModel.Priorities),
			ProjectAdmins = _mapperService.ConvertListTo<ProjectAdmin, ProjectAdminResponse>(fromModel.ProjectAdmins),
			ProjectOwnerUserId = fromModel.ProjectOwnerUserId,
			ProjectUsers = _mapperService.ConvertListTo<ProjectUser, ProjectUserResponse>(fromModel.ProjectUsers),
			Statuses = _mapperService.ConvertListTo<Status, StatusResponse>(fromModel.Statuses),
			Tasks = _mapperService.ConvertListTo<ProjectTask, ProjectTaskResponse>(fromModel.Tasks)
		};
	}
}
