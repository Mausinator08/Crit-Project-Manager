using CritDTO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CritDataAccess.Contexts;

public interface ITenantDbContext
{
    DbSet<Project> Projects { get; set; }
    DbSet<Status> Statuses { get; set; }
    DbSet<Priority> Priorities { get; set; }
    DbSet<CustomFieldType> CustomFieldTypes { get; set; }
    DbSet<CustomField> CustomFields { get; set; }
    DbSet<ProjectTask> Tasks { get; set; }

    void Configure(Action<DatabaseFacade> databaseAction);
}
