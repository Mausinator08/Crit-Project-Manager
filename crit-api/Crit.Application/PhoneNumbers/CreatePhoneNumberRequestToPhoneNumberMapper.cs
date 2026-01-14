using Crit.Contracts.RequestModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class CreatePhoneNumberRequestToPhoneNumberMapper : IMapper<CreatePhoneNumberRequest, PhoneNumber>
{
	public PhoneNumber ConvertTo(CreatePhoneNumberRequest fromModel)
	{
		return new PhoneNumber()
		{
			CountryCode = fromModel.CountryCode,
			Extension = fromModel.Extension,
			Number = fromModel.Number,
			OrganizationId = fromModel.OrganizationId,
			Type = fromModel.Type,
			UserId = fromModel.UserId
		};
	}
}
