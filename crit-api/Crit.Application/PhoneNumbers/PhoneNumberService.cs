using Crit.Application.Mappers;
using Crit.Application.RepositoryInterfaces;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.PhoneNumbers;

public class PhoneNumberService : IPhoneNumberService
{
	private readonly IPhoneNumberRepository _phoneNumberRepository;
	private readonly IMapperService _mapperService;

	public PhoneNumberService(IPhoneNumberRepository phoneNumberRepository, IMapperService mapperService)
	{
		_phoneNumberRepository = phoneNumberRepository;
		_mapperService = mapperService;
	}

	public async Task<PhoneNumberResponse> GetPhoneNumberById(Guid phoneNumberId)
	{
		return _mapperService.ConvertTo<PhoneNumber, PhoneNumberResponse>(await _phoneNumberRepository.GetPhoneNumberById(phoneNumberId));
	}

	public async Task<PhoneNumberResponse> GetPhoneNumberByUserId(Guid userId)
	{
		return _mapperService.ConvertTo<PhoneNumber, PhoneNumberResponse>(await _phoneNumberRepository.GetPhoneNumberByUserId(userId));
	}

	public async Task<PhoneNumberResponse> CreatePhoneNumber(CreatePhoneNumberRequest phoneNumber)
	{
		if ((await _phoneNumberRepository.GetPhoneNumberByNumber(phoneNumber.Number)) != null)
		{
			throw new InvalidOperationException($"Failed to create phoneNumber. The phone number {phoneNumber.Number} already exists.");
		}

		PhoneNumber createdPhoneNumber = await _phoneNumberRepository.CreatePhoneNumber(_mapperService.ConvertTo<CreatePhoneNumberRequest, PhoneNumber>(phoneNumber));

		return _mapperService.ConvertTo<PhoneNumber, PhoneNumberResponse>(createdPhoneNumber);
	}

	public async Task<PhoneNumberResponse> UpdatePhoneNumber(Guid phoneNumberId, UpdatePhoneNumberRequest phoneNumber)
	{
		PhoneNumber phoneNumberToUpdate = await _phoneNumberRepository.GetPhoneNumberById(phoneNumberId);

		phoneNumberToUpdate.Number = phoneNumber.Number ?? phoneNumberToUpdate.Number;
		phoneNumberToUpdate.Extension = phoneNumber.Extension ?? phoneNumberToUpdate.Extension;
		phoneNumberToUpdate.CountryCode = phoneNumber.CountryCode ?? phoneNumberToUpdate.CountryCode;
		phoneNumberToUpdate.Type = phoneNumber.Type ?? phoneNumberToUpdate.Type;
		phoneNumberToUpdate.OrganizationId = phoneNumber.OrganizationId ?? phoneNumberToUpdate.OrganizationId;
		phoneNumberToUpdate.UserId = phoneNumber.UserId ?? phoneNumberToUpdate.UserId;

		PhoneNumber updatedPhoneNumber = await _phoneNumberRepository.UpdatePhoneNumber(phoneNumberToUpdate);

		return _mapperService.ConvertTo<PhoneNumber, PhoneNumberResponse>(updatedPhoneNumber);
	}

	public async Task DeletePhoneNumber(Guid phoneNumberId)
	{
		await _phoneNumberRepository.DeletePhoneNumber(phoneNumberId);
	}
}
