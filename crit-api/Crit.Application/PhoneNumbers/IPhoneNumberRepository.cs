using Crit.Contracts.RequestModels;
using Crit.Domain.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IPhoneNumberRepository
{
    Task<PhoneNumber> GetPhoneNumberById(Guid phoneNumberId);
    Task<PhoneNumber> GetPhoneNumberByUserId(Guid userId);
    Task<PhoneNumber?> GetPhoneNumberByNumber(string number);
    Task<PhoneNumber> CreatePhoneNumber(PhoneNumber phoneNumber);
    Task UpdatePhoneNumber(PhoneNumber phoneNumber);
    Task DeletePhoneNumber(Guid phoneNumberId);
}
