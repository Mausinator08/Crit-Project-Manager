using Crit.Contracts.Emails;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class CreateEmailRequestToEmailMapper : IMapper<CreateEmailRequest, Email>
{
	public Email ConvertTo(CreateEmailRequest fromModel)
	{
		return new Email()
		{
			EmailAddress = fromModel.EmailAddress,
			OrganizationId = fromModel.OrganizationId,
			UserId = fromModel.UserId
		};
	}
}
