using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Models;
using Crit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Crit.Infrastructure.Repositories;

public class EmailRepository : IEmailRepository
{
    private readonly CritDbContext _critDbContext;

    public EmailRepository(CritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
    }

    public async Task<Email> GetEmailById(Guid emailId)
    {
        Email? email = await _critDbContext.Emails.AsNoTracking().FirstOrDefaultAsync(e => e.Id == emailId);
        if (email == null)
        {
            throw new Exception("Email not found");
        }

        return email;
    }

    public async Task<Email?> GetEmailByAddress(string address)
    {
        return await _critDbContext.Emails.AsNoTracking().FirstOrDefaultAsync(e => e.EmailAddress == address);
    }

    public async Task<Email> CreateEmail(Email email)
    {
        Email newEmail = new Email()
        {
            OrganizationId = email.OrganizationId,
            EmailAddress = email.EmailAddress,
            UserId = email.UserId
        };

        EntityEntry<Email> createdEmail = await _critDbContext.Emails.AddAsync(newEmail);
        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to create Email.");
        }

        return createdEmail.Entity;
    }

    public async Task<Email> UpdateEmail(Email email)
    {
        EntityEntry<Email> updatedEmail = _critDbContext.Emails.Update(email);
        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to save Email.");
        }

        return updatedEmail.Entity;
    }

    public async Task DeleteEmail(Guid emailId)
    {
        Email? email = await _critDbContext.Emails.FindAsync(emailId);
        if (email == null)
        {
            throw new Exception("Email not found");
        }

        _critDbContext.Emails.Remove(email);
        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to delete Email.");
        }
    }
}
