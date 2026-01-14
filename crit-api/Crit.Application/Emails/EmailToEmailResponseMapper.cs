using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class EmailToEmailResponseMapper : IMapper<Email, EmailResponse>
{
	public EmailResponse ConvertTo(Email fromModel)
	{
		return new EmailResponse()
		{
			Id = fromModel.Id,
			EmailAddress = fromModel.EmailAddress,
			OrganizationId = fromModel.OrganizationId,
			UserId = fromModel.UserId
		};
	}
}
