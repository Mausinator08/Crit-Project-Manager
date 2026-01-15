using Crit.Contracts.RequestModels;
using Crit.Domain.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IPhoneNumberRepository
{
    Task<PhoneNumber> GetPhoneNumberById(Guid phoneNumberId);
    Task<PhoneNumber> GetPhoneNumberByUserId(Guid userId);
    Task<PhoneNumber?> GetPhoneNumberByNumber(string number);
    Task<PhoneNumber> CreatePhoneNumber(PhoneNumber phoneNumber);
    Task<PhoneNumber> UpdatePhoneNumber(PhoneNumber phoneNumber);
    Task DeletePhoneNumber(Guid phoneNumberId);
}
