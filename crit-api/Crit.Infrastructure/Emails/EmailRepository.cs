using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Models;
using Crit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Crit.Infrastructure.Repositories;

public class EmailRepository : IEmailRepository
{
    private readonly CritDbContext _critDbContext;

    public EmailRepository(CritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
    }

    public async Task<Email?> CreateEmail(Email email)
    {
        Email? newEmail = new Email()
        {
            OrganizationId = email.OrganizationId,
            EmailAddress = email.EmailAddress,
            UserId = email.UserId
        };

        await _critDbContext.Emails.AddAsync(newEmail);
        await _critDbContext.SaveChangesAsync();

        return newEmail;
    }

    public async Task DeleteEmail(Guid emailId)
    {
        Email? email = await _critDbContext.Emails.FindAsync(emailId);
        if (email == null)
        {
            throw new Exception("Email not found");
        }

        _critDbContext.Emails.Remove(email);
        await _critDbContext.SaveChangesAsync();
    }

    public async Task<Email> GetEmailById(Guid emailId)
    {
        Email? email = await _critDbContext.Emails.FindAsync(emailId);
        if (email == null)
        {
            throw new Exception("Email not found");
        }

        return email;
    }

    public async Task<Email?> GetEmailByAddress(string address)
    {
        return await _critDbContext.Emails.FirstOrDefaultAsync(e => e.EmailAddress == address);
    }

    public async Task UpdateEmail(Email email)
    {
        Email? existingEmail = await _critDbContext.Emails.FindAsync(email.Id);
        if (existingEmail == null)
        {
            throw new Exception("Email not found");
        }

        existingEmail.EmailAddress = email.EmailAddress;
        existingEmail.UserId = email.UserId;

        _critDbContext.Emails.Update(existingEmail);
        await _critDbContext.SaveChangesAsync();
    }

}
