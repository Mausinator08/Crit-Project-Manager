using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class StatusToStatusResponseMapper : IMapper<Status, StatusResponse>
{
	public StatusResponse ConvertTo(Status fromModel)
	{
		return new StatusResponse()
		{
			Id = fromModel.Id,
			Name = fromModel.Name,
			Description = fromModel.Description,
			BackgroundColor = fromModel.BackgroundColor,
			Color = fromModel.Color,
			ProjectId = fromModel.ProjectId,
			TaskId = fromModel.TaskId
		};
	}
}
