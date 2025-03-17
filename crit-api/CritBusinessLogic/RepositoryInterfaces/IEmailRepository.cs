using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IEmailRepository
{
    Task<Email> GetEmailById(string emailId);
    Task<Email> CreateEmail(Email email);
    System.Threading.Tasks.Task UpdateEmail(Email email);
    System.Threading.Tasks.Task DeleteEmail(string emailId);
}
