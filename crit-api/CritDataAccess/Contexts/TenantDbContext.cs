using CritDataAccess.ValueGenerators;
using CritDTO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CritDataAccess.Contexts;

public class TenantDbContext : DbContext, ITenantDbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Priority> Priorities { get; set; }
    public DbSet<CustomFieldType> CustomFieldTypes { get; set; }
    public DbSet<CustomField> CustomFields { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }

    public TenantDbContext(DbContextOptions options) : base(options)
    {
        ConfigureDefault();
    }

    protected TenantDbContext()
    {
        ConfigureDefault();
    }

    public void Configure(Action<DatabaseFacade> databaseAction)
    {
        databaseAction.Invoke(Database);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToCollection("Projects");
            entity.HasKey(collection => collection.Id);
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(true)
            .HasConversion(id => ObjectId.Parse(id), oid => oid.ToString())
            .HasValueGenerator<ObjectIdValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(collection => collection.Name).HasColumnName("name").IsRequired(true);
            entity.Property(collection => collection.Description).HasColumnName("description").IsRequired(false);
            entity.Property(collection => collection.OwningOrganizationId).HasColumnName("owningOrganizationId").IsRequired(true);
            entity.Property(collection => collection.ProjectOwnerUserId).HasColumnName("projectOwnerUserId").IsRequired(true);
            entity.Property(collection => collection.CreatedByUserId).HasColumnName("createdByUserId").IsRequired(true);
            entity.Property(collection => collection.DateCreated).HasColumnName("dateCreated").IsRequired(true);
            entity.Property(collection => collection.DateUpdated).HasColumnName("dateUpdated").IsRequired(true);
            entity.Property(collection => collection.OrganizationIds).HasColumnName("organizationIds").IsRequired(false);
            entity.Property(collection => collection.ProjectAdminUserIds).HasColumnName("projectAdminUserIds").IsRequired(false);
            entity.Property(collection => collection.ProjectUserIds).HasColumnName("projectUserIds").IsRequired(false);
            entity.Property(collection => collection.UpdatedByUserId).HasColumnName("updatedByUserId").IsRequired(true);
            entity.Property(collection => collection.HiddenCustomFieldTypeIds).HasColumnName("hiddenCustomFieldTypeIds").IsRequired(false);
        });

        modelBuilder.Entity<Project>()
        .HasMany(e => e.CustomFieldTypes)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Project>()
        .HasMany(e => e.Priorities)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Project>()
        .HasMany(e => e.Statuses)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Project>()
        .HasMany(e => e.Tasks)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToCollection("Statuses");
            entity.HasKey(collection => collection.Id);
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(true)
            .HasConversion(id => ObjectId.Parse(id), oid => oid.ToString())
            .HasValueGenerator<ObjectIdValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(collection => collection.BackgroundColor).HasColumnName("backgroundColor").IsRequired(false);
            entity.Property(collection => collection.Color).HasColumnName("color").IsRequired(false);
            entity.Property(collection => collection.Description).HasColumnName("description").IsRequired(false);
            entity.Property(collection => collection.Name).HasColumnName("name").IsRequired(true);
            entity.Property(collection => collection.ProjectId).HasColumnName("projectId").IsRequired(true);
        });

        modelBuilder.Entity<Status>()
        .HasOne(e => e.Project)
        .WithMany(e => e.Statuses)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Status>()
        .HasMany(e => e.Tasks)
        .WithOne(e => e.Status)
        .HasForeignKey(e => e.StatusId)
        .IsRequired(false);

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.ToCollection("Priorities");
            entity.HasKey(collection => collection.Id);
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(true)
            .HasConversion(id => ObjectId.Parse(id), oid => oid.ToString())
            .HasValueGenerator<ObjectIdValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(collection => collection.Name).HasColumnName("name").IsRequired(true);
            entity.Property(collection => collection.BackgroundColor).HasColumnName("backgroundColor").IsRequired(false);
            entity.Property(collection => collection.Color).HasColumnName("color").IsRequired(false);
            entity.Property(collection => collection.ProjectId).HasColumnName("projectId").IsRequired(true);
        });

        modelBuilder.Entity<Priority>()
        .HasOne(e => e.Project)
        .WithMany(e => e.Priorities)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Priority>()
        .HasMany(e => e.Tasks)
        .WithOne(e => e.Priority)
        .HasForeignKey(e => e.PriorityId)
        .IsRequired(false);

        modelBuilder.Entity<CustomFieldType>(entity =>
        {
            entity.ToCollection("CustomFieldTypes");
            entity.HasKey(collection => collection.Id);
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(true)
            .HasConversion(id => ObjectId.Parse(id), oid => oid.ToString())
            .HasValueGenerator<ObjectIdValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(collection => collection.Name).HasColumnName("name").IsRequired(true);
            entity.Property(collection => collection.ProjectId).HasColumnName("projectId").IsRequired(true);
        });

        modelBuilder.Entity<CustomFieldType>()
        .HasOne(e => e.Project)
        .WithMany(e => e.CustomFieldTypes)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<ProjectTask>(entity =>
        {
            entity.ToCollection("Tasks");
            entity.HasKey(collection => collection.Id);
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(true)
            .HasConversion(id => ObjectId.Parse(id), oid => oid.ToString())
            .HasValueGenerator<ObjectIdValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(collection => collection.Title).HasColumnName("title").IsRequired(false);
            entity.Property(collection => collection.Details).HasColumnName("details").IsRequired(false);
            entity.Property(collection => collection.ProjectId).HasColumnName("projectId").IsRequired(true);
            entity.Property(collection => collection.AssignedUserId).HasColumnName("assignedUserId").IsRequired(false);
            entity.Property(collection => collection.StatusId).HasColumnName("statusId").IsRequired(false);
            entity.Property(collection => collection.PriorityId).HasColumnName("priorityId").IsRequired(false);
            entity.Property(collection => collection.Complexity).HasColumnName("complexity").IsRequired(false);
            entity.Property(collection => collection.DueDate).HasColumnName("dueDate").IsRequired(false);
            entity.Property(collection => collection.ParentTaskId).HasColumnName("parentTaskId").IsRequired(false);
        });

        modelBuilder.Entity<ProjectTask>()
        .HasOne(e => e.Status)
        .WithMany(e => e.Tasks)
        .HasForeignKey(e => e.StatusId)
        .IsRequired(false);

        modelBuilder.Entity<ProjectTask>()
        .HasOne(e => e.Priority)
        .WithMany(e => e.Tasks)
        .HasForeignKey(e => e.PriorityId)
        .IsRequired(false);

        modelBuilder.Entity<ProjectTask>()
        .HasMany(e => e.CustomFields)
        .WithOne(e => e.Task)
        .HasForeignKey(e => e.TaskId)
        .IsRequired(true);

        modelBuilder.Entity<ProjectTask>()
        .HasOne(e => e.Project)
        .WithMany(e => e.Tasks)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<ProjectTask>()
        .HasMany(e => e.SubTasks)
        .WithOne(e => e.ParentTask)
        .HasForeignKey(e => e.ParentTaskId)
        .IsRequired(false);

        modelBuilder.Entity<CustomField>(entity =>
        {
            entity.ToCollection("CustomFields");
            entity.HasKey(collection => collection.Id);
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(true)
            .HasConversion(id => ObjectId.Parse(id), oid => oid.ToString())
            .HasValueGenerator<ObjectIdValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(collection => collection.Value).HasColumnName("value").IsRequired(true);
            entity.Property(collection => collection.CustomFieldTypeId).HasColumnName("customFieldTypeId").IsRequired(true);
            entity.Property(collection => collection.TaskId).HasColumnName("taskId").IsRequired(true);
        });

        modelBuilder.Entity<CustomField>()
        .HasOne(e => e.CustomFieldType)
        .WithMany(e => e.CustomFields)
        .HasForeignKey(e => e.CustomFieldTypeId)
        .IsRequired(true);

        modelBuilder.Entity<CustomField>()
        .HasOne(e => e.Task)
        .WithMany(e => e.CustomFields)
        .HasForeignKey(e => e.TaskId)
        .IsRequired(true);
    }

    private void ConfigureDefault()
    {
        Configure(db =>
        {
            db.EnsureCreated();
        });
    }
}
