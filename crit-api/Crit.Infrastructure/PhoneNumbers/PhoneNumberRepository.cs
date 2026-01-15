using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Models;
using Crit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Crit.Infrastructure.Repositories;

public class PhoneNumberRepository : IPhoneNumberRepository
{
    private readonly CritDbContext _critDbContext;

    public PhoneNumberRepository(CritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
    }

    public async Task<PhoneNumber> GetPhoneNumberByUserId(Guid userId)
    {
        List<PhoneNumber> phoneNumber = await _critDbContext.PhoneNumbers.AsNoTracking().Where(p => p.UserId == userId).ToListAsync();
        if (!phoneNumber.Any())
        {
            throw new Exception($"Phone number not found for user id {userId}");
        }

        return phoneNumber.First();
    }

    public async Task<PhoneNumber> GetPhoneNumberById(Guid phoneNumberId)
    {
        PhoneNumber? phoneNumber = await _critDbContext.PhoneNumbers.AsNoTracking().FirstOrDefaultAsync(pn => pn.Id == phoneNumberId);
        if (phoneNumber == null)
        {
            throw new Exception("Phone number not found");
        }

        return phoneNumber;
    }

    public async Task<PhoneNumber?> GetPhoneNumberByNumber(string number)
    {
        return await _critDbContext.PhoneNumbers.AsNoTracking().FirstOrDefaultAsync(e => e.Number == number);
    }

    public async Task<PhoneNumber> CreatePhoneNumber(PhoneNumber phoneNumber)
    {
        PhoneNumber newPhoneNumber = new PhoneNumber()
        {
            OrganizationId = phoneNumber.OrganizationId,
            Number = phoneNumber.Number,
            Extension = phoneNumber.Extension,
            CountryCode = phoneNumber.CountryCode,
            Type = phoneNumber.Type,
            Organization = phoneNumber.Organization,
            UserId = phoneNumber.UserId
        };

        EntityEntry<PhoneNumber> createdPhoneNumber = _critDbContext.PhoneNumbers.Add(newPhoneNumber);
        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to create Phone Number.");
        }

        return createdPhoneNumber.Entity;
    }

    public async Task<PhoneNumber> UpdatePhoneNumber(PhoneNumber phoneNumber)
    {
        EntityEntry<PhoneNumber> updatedPhoneNumber = _critDbContext.PhoneNumbers.Update(phoneNumber);
        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to save Phone Number.");
        }

        return updatedPhoneNumber.Entity;
    }

    public async Task DeletePhoneNumber(Guid phoneNumberId)
    {
        var phoneNumber = await _critDbContext.PhoneNumbers.FindAsync(phoneNumberId);
        if (phoneNumber == null)
        {
            throw new Exception("Phone number not found");
        }

        _critDbContext.PhoneNumbers.Remove(phoneNumber);
        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to delete Phone Number.");
        }
    }
}
