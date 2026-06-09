using Crit.Domain.Entities;
using Crit.Domain.Modules.OrganizationManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Crit.Application.Modules.OrganizationManagement.Contracts.Persistence;

public interface ICritOrganizationManagementContext
{
    DbSet<Organization> Organizations { get; set; }

    void Configure(Action<DatabaseFacade> databaseAction);
}
