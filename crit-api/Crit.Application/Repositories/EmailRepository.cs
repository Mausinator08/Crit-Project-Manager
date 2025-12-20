using Crit.Application.RepositoryInterfaces;
using Crit.Contracts.Models;
using Crit.Infrastructure.Contexts;

namespace Crit.Application.Repositories;

public class EmailRepository : IEmailRepository
{
    private readonly CritDbContext _critDbContext;

    public EmailRepository(CritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
    }

    public async Task<Email> CreateEmail(Email email)
    {
        try
        {
            var newEmail = new Email()
            {
                OrganizationId = email.OrganizationId,
                EmailAddress = email.EmailAddress,
                UserId = email.UserId
            };

            await _critDbContext.Emails.AddAsync(newEmail);
            await _critDbContext.SaveChangesAsync();

            return newEmail;
        }
        catch (Exception e)
        {
            throw new Exception("Error creating email", e);
        }
    }

    public async Task DeleteEmail(Guid emailId)
    {
        try
        {
            var email = await _critDbContext.Emails.FindAsync(emailId);
            if (email == null)
            {
                throw new Exception("Email not found");
            }

            _critDbContext.Emails.Remove(email);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Error deleting email", e);
        }
    }

    public async Task<Email> GetEmailById(Guid emailId)
    {
        try
        {
            var email = await _critDbContext.Emails.FindAsync(emailId);
            if (email == null)
            {
                throw new Exception("Email not found");
            }

            return email;
        }
        catch (Exception e)
        {
            throw new Exception("Error getting email", e);
        }
    }

    public async Task UpdateEmail(Email email)
    {
        try
        {
            var existingEmail = await _critDbContext.Emails.FindAsync(email.Id);
            if (existingEmail == null)
            {
                throw new Exception("Email not found");
            }

            existingEmail.EmailAddress = email.EmailAddress;
            existingEmail.UserId = email.UserId;

            _critDbContext.Emails.Update(existingEmail);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Error updating email", e);
        }
    }

}
