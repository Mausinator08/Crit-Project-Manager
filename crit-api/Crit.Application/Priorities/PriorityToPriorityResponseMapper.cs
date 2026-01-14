using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class PriorityToPriorityResponseMapper : IMapper<Priority, PriorityResponse>
{
	public PriorityResponse ConvertTo(Priority fromModel)
	{
		return new PriorityResponse()
		{
			Id = fromModel.Id,
			Name = fromModel.Name,
			BackgroundColor = fromModel.BackgroundColor,
			Color = fromModel.Color,
			ProjectId = fromModel.ProjectId,
			TaskId = fromModel.TaskId
		};
	}
}
