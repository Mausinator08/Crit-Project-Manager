using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;

namespace Crit.Application.PhoneNumbers;

public interface IPhoneNumberService
{
	Task<PhoneNumberResponse> GetPhoneNumberById(Guid phoneNumberId);
	Task<PhoneNumberResponse> GetPhoneNumberByUserId(Guid userId);
	Task<PhoneNumberResponse> CreatePhoneNumber(CreatePhoneNumberRequest phoneNumber);
	Task<PhoneNumberResponse> UpdatePhoneNumber(Guid phoneNumberId, UpdatePhoneNumberRequest phoneNumber);
	Task DeletePhoneNumber(Guid phoneNumberId);
}
