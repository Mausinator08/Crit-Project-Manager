using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class TaskToTaskResponse : IMapper<ProjectTask, ProjectTaskResponse>
{
	private readonly IMapperService _mapperService;

	public TaskToTaskResponse(IMapperService mapperService)
	{
		_mapperService = mapperService;
	}

	public ProjectTaskResponse ConvertTo(ProjectTask fromModel)
	{
		return new ProjectTaskResponse()
		{
			Id = fromModel.Id,
			AssignedUserId = fromModel.AssignedUserId,
			Comments = _mapperService.ConvertListTo<Comment, CommentResponse>(fromModel.Comments),
			Complexity = fromModel.Complexity,
			CustomFields = _mapperService.ConvertListTo<CustomField, CustomFieldResponse>(fromModel.CustomFields),
			Details = fromModel.Details,
			DueDate = fromModel.DueDate,
			ParentTaskId = fromModel.ParentTaskId,
			Priority = fromModel.Priority != null ? _mapperService.ConvertTo<Priority, PriorityResponse>(fromModel.Priority) : null,
			PriorityId = fromModel.PriorityId,
			ProjectId = fromModel.ProjectId,
			Status = fromModel.Status != null ? _mapperService.ConvertTo<Status, StatusResponse>(fromModel.Status) : null,
			StatusId = fromModel.StatusId,
			SubTasks = _mapperService.ConvertListTo<ProjectTask, ProjectTaskResponse>(fromModel.SubTasks),
			Title = fromModel.Title
		};
	}
}
