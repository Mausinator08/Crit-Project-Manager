using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDTO.Models;

namespace CritBusinessLogic.Repositories;

public class PhoneNumberRepository : IPhoneNumberRepository
{
    private readonly CritDbContext _critDbContext;

    public PhoneNumberRepository(CritDbContext critDbContext)
    {
        _critDbContext = critDbContext;
    }

    public async Task<PhoneNumber> CreatePhoneNumber(PhoneNumber phoneNumber)
    {
        try
        {
            var newPhoneNumber = new PhoneNumber()
            {
                OrganizationId = phoneNumber.OrganizationId,
                Number = phoneNumber.Number,
                Extension = phoneNumber.Extension,
                CountryCode = phoneNumber.CountryCode,
                Type = phoneNumber.Type,
                Organization = phoneNumber.Organization
            };

            _critDbContext.PhoneNumbers.Add(newPhoneNumber);
            await _critDbContext.SaveChangesAsync();

            return newPhoneNumber;
        }
        catch (Exception e)
        {
            throw new Exception("Error creating phone number", e);
        }
    }

    public async System.Threading.Tasks.Task DeletePhoneNumber(string phoneNumberId)
    {
        try
        {
            var phoneNumber = await _critDbContext.PhoneNumbers.FindAsync(phoneNumberId);
            if (phoneNumber == null)
            {
                throw new Exception("Phone number not found");
            }

            _critDbContext.PhoneNumbers.Remove(phoneNumber);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Error deleting phone number", e);
        }
    }

    public async Task<PhoneNumber> GetPhoneNumberById(string phoneNumberId)
    {
        try
        {
            var phoneNumber = await _critDbContext.PhoneNumbers.FindAsync(phoneNumberId);
            if (phoneNumber == null)
            {
                throw new Exception("Phone number not found");
            }

            return phoneNumber;
        }
        catch (Exception e)
        {
            throw new Exception("Error getting phone number", e);
        }
    }

    public async System.Threading.Tasks.Task UpdatePhoneNumber(PhoneNumber phoneNumber)
    {
        try
        {
            var phoneNumberToUpdate = await _critDbContext.PhoneNumbers.FindAsync(phoneNumber.Id);
            if (phoneNumberToUpdate == null)
            {
                throw new Exception("Phone number not found");
            }

            phoneNumberToUpdate.Number = phoneNumber.Number;
            phoneNumberToUpdate.Extension = phoneNumber.Extension;
            phoneNumberToUpdate.CountryCode = phoneNumber.CountryCode;
            phoneNumberToUpdate.Type = phoneNumber.Type;
            phoneNumberToUpdate.OrganizationId = phoneNumber.OrganizationId;
            phoneNumberToUpdate.Organization = phoneNumber.Organization;

            _critDbContext.PhoneNumbers.Update(phoneNumberToUpdate);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Error updating phone number", e);
        }
    }

}
