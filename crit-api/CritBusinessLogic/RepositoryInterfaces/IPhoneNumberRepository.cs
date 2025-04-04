using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IPhoneNumberRepository
{
    Task<PhoneNumber> GetPhoneNumberById(string phoneNumberId);
    Task<PhoneNumber> GetPhoneNumberByUserId(string userId);
    Task<PhoneNumber> CreatePhoneNumber(PhoneNumber phoneNumber);
    System.Threading.Tasks.Task UpdatePhoneNumber(PhoneNumber phoneNumber);
    System.Threading.Tasks.Task DeletePhoneNumber(string phoneNumberId);
}
