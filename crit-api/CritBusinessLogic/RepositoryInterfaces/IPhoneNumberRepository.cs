using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IPhoneNumberRepository
{
    Task<PhoneNumber> GetPhoneNumberById(Guid phoneNumberId);
    Task<PhoneNumber> CreatePhoneNumber(PhoneNumber phoneNumber);
    System.Threading.Tasks.Task UpdatePhoneNumber(PhoneNumber phoneNumber);
    System.Threading.Tasks.Task DeletePhoneNumber(Guid phoneNumberId);
}
