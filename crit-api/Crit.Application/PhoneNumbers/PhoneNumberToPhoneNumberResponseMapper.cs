using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class PhoneNumberToPhoneNumberResponseMapper : IMapper<PhoneNumber, PhoneNumberResponse>
{
	public PhoneNumberResponse ConvertTo(PhoneNumber fromModel)
	{
		return new PhoneNumberResponse()
		{
			Id = fromModel.Id,
			CountryCode = fromModel.CountryCode,
			Extension = fromModel.Extension,
			Number = fromModel.Number,
			OrganizationId = fromModel.OrganizationId,
			Type = fromModel.Type,
			UserId = fromModel.UserId
		};
	}
}
