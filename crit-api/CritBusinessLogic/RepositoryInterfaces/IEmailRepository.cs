using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IEmailRepository
{
    Task<Email> GetEmailById(Guid emailId);
    Task<Email> CreateEmail(Email email);
    System.Threading.Tasks.Task UpdateEmail(Email email);
    System.Threading.Tasks.Task DeleteEmail(Guid emailId);
}
