using CritDTO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CritDataAccess.Contexts;

public interface ICritDbContext
{
    DbSet<Organization> Organizations { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    void Configure(Action<DatabaseFacade> databaseAction);
}
