using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CritDataAccess.Contexts;

public interface ICritDbContext
{
    void Configure(Action<DatabaseFacade> databaseAction);
}
