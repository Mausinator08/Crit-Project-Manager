using Crit.Abstractions.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IPhoneNumberRepository
{
    Task<PhoneNumber> GetPhoneNumberById(Guid phoneNumberId);
    Task<PhoneNumber> GetPhoneNumberByUserId(Guid userId);
    Task<PhoneNumber> CreatePhoneNumber(PhoneNumber phoneNumber);
    Task UpdatePhoneNumber(PhoneNumber phoneNumber);
    Task DeletePhoneNumber(Guid phoneNumberId);
}
