using Crit.Contracts.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IEmailRepository
{
    Task<Email> GetEmailById(Guid emailId);
    Task<Email> CreateEmail(Email email);
    Task UpdateEmail(Email email);
    Task DeleteEmail(Guid emailId);
}
