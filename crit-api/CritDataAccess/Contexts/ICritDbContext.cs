using CritDTO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CritDataAccess.Contexts;

public interface ICritDbContext
{
    DbSet<Organization> Organizations { get; set; }
    void Configure(Action<DatabaseFacade> databaseAction);
}
