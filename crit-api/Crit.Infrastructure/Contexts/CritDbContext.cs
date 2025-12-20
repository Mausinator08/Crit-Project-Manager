using Crit.Domain.Entities;
using Crit.Domain.Identity;
using Crit.Infrastructure.ValueGenerators;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Crit.Domain.Contexts;

public class CritDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, ICritDbContext
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Priority> Priorities { get; set; }
    public DbSet<CustomFieldType> CustomFieldTypes { get; set; }
    public DbSet<CustomField> CustomFields { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentQuote> CommentQuotes { get; set; }
    public DbSet<MentionedUserComment> MentionedUserComments { get; set; }
    public DbSet<OrganizationProject> OrganizationProjects { get; set; }
    public DbSet<OrganizationAdmin> OrganizationAdmins { get; set; }
    public DbSet<OrganizationMember> OrganizationMembers { get; set; }
    public DbSet<OrganizationAffiliate> OrganizationAffiliates { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }
    public DbSet<ProjectAdmin> ProjectAdmins { get; set; }

    public CritDbContext(DbContextOptions options) : base(options)
    {
    }

    public void Configure(Action<DatabaseFacade> databaseAction)
    {
        databaseAction.Invoke(Database);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("Organization");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.Name).HasColumnName("Name").IsRequired(true);
            entity.Property(table => table.OwnerUserId).HasColumnName("OwnerUserId").IsRequired(true);
        });

        modelBuilder.Entity<Organization>()
        .HasMany(table => table.Emails)
        .WithOne(table => table.Organization)
        .HasForeignKey(table => table.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Organization>()
        .HasMany(table => table.PhoneNumbers)
        .WithOne(table => table.Organization)
        .HasForeignKey(table => table.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Organization>()
        .HasMany(table => table.OrganizationProjects)
        .WithOne(table => table.Organization)
        .HasForeignKey(table => table.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Organization>()
        .HasMany(table => table.OrganizationAdmins)
        .WithOne(table => table.Organization)
        .HasForeignKey(table => table.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Organization>()
        .HasMany(table => table.OrganizationMembers)
        .WithOne(table => table.Organization)
        .HasForeignKey(table => table.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Organization>()
        .HasMany(table => table.OrganizationAffiliates)
        .WithOne(table => table.Organization)
        .HasForeignKey(table => table.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Organization>()
        .HasOne(table => table.Owner)
        .WithMany(table => table.OwnedOrganizations)
        .HasForeignKey(table => table.OwnerUserId)
        .IsRequired(true);

        modelBuilder.Entity<Organization>()
        .HasMany(table => table.Projects)
        .WithOne(table => table.OwningOrganization)
        .HasForeignKey(table => table.OwningOrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Email>(entity =>
        {
            entity.ToTable("Email");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.EmailAddress).HasColumnName("EmailAddress").IsRequired(false);
            entity.Property(table => table.UserId).HasColumnName("UserId").IsRequired(false);
            entity.Property(table => table.OrganizationId).HasColumnName("OrganizationId").IsRequired(true);
        });

        modelBuilder.Entity<Email>()
        .HasOne(table => table.Organization)
        .WithMany(table => table.Emails)
        .HasForeignKey(table => table.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.ToTable("PhoneNumber");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.CountryCode).HasColumnName("CountryCode").IsRequired(true);
            entity.Property(table => table.Number).HasColumnName("Number").IsRequired(true);
            entity.Property(table => table.Extension).HasColumnName("Extension").IsRequired(false);
            entity.Property(table => table.Type).HasColumnName("Type").IsRequired(true);
            entity.Property(table => table.UserId).HasColumnName("UserId").IsRequired(false);
        });

        modelBuilder.Entity<PhoneNumber>()
        .HasOne(table => table.Organization)
        .WithMany(table => table.PhoneNumbers)
        .HasForeignKey(table => table.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.Name).HasColumnName("Name").IsRequired(true);
            entity.Property(table => table.Description).HasColumnName("Description").IsRequired(false);
            entity.Property(table => table.OwningOrganizationId).HasColumnName("OwningOrganizationId").IsRequired(true);
            entity.Property(table => table.ProjectOwnerUserId).HasColumnName("ProjectOwnerUserId").IsRequired(true);
            entity.Property(table => table.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired(true);
            entity.Property(table => table.DateCreated).HasColumnName("DateCreated").IsRequired(true);
            entity.Property(table => table.DateUpdated).HasColumnName("DateUpdated").IsRequired(true);
            entity.Property(table => table.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired(true);
        });

        modelBuilder.Entity<Project>()
        .HasMany(e => e.CustomFieldTypes)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Project>()
        .HasMany(e => e.HiddenCustomFieldTypes)
        .WithOne(e => e.HiddenProject)
        .HasForeignKey(e => e.HiddenProjectId)
        .IsRequired(false);

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

        modelBuilder.Entity<Project>()
        .HasMany(e => e.Comments)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Project>()
        .HasMany(e => e.OrganizationProjects)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Project>()
        .HasOne(e => e.OwningOrganization)
        .WithMany(e => e.Projects)
        .HasForeignKey(e => e.OwningOrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Project>()
        .HasOne(e => e.ProjectOwner)
        .WithMany(e => e.Projects)
        .HasForeignKey(e => e.ProjectOwnerUserId)
        .IsRequired(true);

        modelBuilder.Entity<Project>()
        .HasMany(e => e.ProjectUsers)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Project>()
        .HasMany(e => e.ProjectAdmins)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);


        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.BackgroundColor).HasColumnName("BackgroundColor").IsRequired(false);
            entity.Property(table => table.Color).HasColumnName("Color").IsRequired(false);
            entity.Property(table => table.Description).HasColumnName("Description").IsRequired(false);
            entity.Property(table => table.Name).HasColumnName("Name").IsRequired(true);
            entity.Property(table => table.ProjectId).HasColumnName("ProjectId").IsRequired(true);
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
            entity.ToTable("Priority");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.Name).HasColumnName("Name").IsRequired(true);
            entity.Property(table => table.BackgroundColor).HasColumnName("BackgroundColor").IsRequired(false);
            entity.Property(table => table.Color).HasColumnName("Color").IsRequired(false);
            entity.Property(table => table.ProjectId).HasColumnName("ProjectId").IsRequired(true);
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
            entity.ToTable("CustomFieldType");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.Name).HasColumnName("Name").IsRequired(true);
            entity.Property(table => table.ProjectId).HasColumnName("ProjectId").IsRequired(true);
            entity.Property(table => table.HiddenProjectId).HasColumnName("HiddenProjectId").IsRequired(false);
        });

        modelBuilder.Entity<CustomFieldType>()
        .HasOne(e => e.Project)
        .WithMany(e => e.CustomFieldTypes)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<CustomFieldType>()
        .HasOne(e => e.HiddenProject)
        .WithMany(e => e.HiddenCustomFieldTypes)
        .HasForeignKey(e => e.HiddenProjectId)
        .IsRequired(false);

        modelBuilder.Entity<ProjectTask>(entity =>
        {
            entity.ToTable("Task");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.Title).HasColumnName("Title").IsRequired(false);
            entity.Property(table => table.Details).HasColumnName("Details").IsRequired(false);
            entity.Property(table => table.ProjectId).HasColumnName("ProjectId").IsRequired(true);
            entity.Property(table => table.AssignedUserId).HasColumnName("AssignedUserId").IsRequired(false);
            entity.Property(table => table.StatusId).HasColumnName("StatusId").IsRequired(false);
            entity.Property(table => table.PriorityId).HasColumnName("PriorityId").IsRequired(false);
            entity.Property(table => table.Complexity).HasColumnName("Complexity").IsRequired(false);
            entity.Property(table => table.DueDate).HasColumnName("DueDate").IsRequired(false);
            entity.Property(table => table.ParentTaskId).HasColumnName("ParentTaskId").IsRequired(false);
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
            entity.ToTable("CustomField");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.Value).HasColumnName("Value").IsRequired(true);
            entity.Property(table => table.CustomFieldTypeId).HasColumnName("CustomFieldTypeId").IsRequired(true);
            entity.Property(table => table.TaskId).HasColumnName("TaskId").IsRequired(true);
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

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.Text).HasColumnName("Text").IsRequired(true);
            entity.Property(table => table.TaskId).HasColumnName("TaskId").IsRequired(false);
            entity.Property(table => table.ProjectId).HasColumnName("ProjectId").IsRequired(true);
        });

        modelBuilder.Entity<Comment>()
        .HasOne(e => e.Project)
        .WithMany(e => e.Comments)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<Comment>()
        .HasOne(e => e.Task)
        .WithMany(e => e.Comments)
        .HasForeignKey(e => e.TaskId)
        .IsRequired(false);

        modelBuilder.Entity<Comment>()
        .HasMany(e => e.QuotedComments)
        .WithOne(e => e.Comment)
        .HasForeignKey(e => e.CommentId)
        .IsRequired(true);

        modelBuilder.Entity<Comment>()
        .HasMany(e => e.MentionedUsers)
        .WithOne(e => e.Comment)
        .HasForeignKey(e => e.CommentId)
        .IsRequired(true);

        modelBuilder.Entity<CommentQuote>(entity =>
        {
            entity.ToTable("CommentQuote");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.CommentId).HasColumnName("CommentId").IsRequired(true);
            entity.Property(table => table.QuotedCommentId).HasColumnName("QuotedCommentId").IsRequired(true);
        });

        modelBuilder.Entity<CommentQuote>()
        .HasOne(e => e.Comment)
        .WithMany(e => e.QuotedComments)
        .HasForeignKey(e => e.CommentId)
        .IsRequired(true);

        modelBuilder.Entity<MentionedUserComment>(entity =>
        {
            entity.ToTable("MentionedUserComment");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.CommentId).HasColumnName("CommentId").IsRequired(true);
            entity.Property(table => table.ApplicationUserId).HasColumnName("ApplicationUserId").IsRequired(true);
        });

        modelBuilder.Entity<MentionedUserComment>()
        .HasOne(e => e.Comment)
        .WithMany(e => e.MentionedUsers)
        .HasForeignKey(e => e.CommentId)
        .IsRequired(true);

        modelBuilder.Entity<MentionedUserComment>()
        .HasOne(e => e.ApplicationUser)
        .WithMany(e => e.MentionedUserComments)
        .HasForeignKey(e => e.ApplicationUserId)
        .IsRequired(true);

        modelBuilder.Entity<OrganizationProject>(entity =>
        {
            entity.ToTable("OrganizationProject");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.OrganizationId).HasColumnName("OrganizationId").IsRequired(true);
            entity.Property(table => table.ProjectId).HasColumnName("ProjectId").IsRequired(true);
        });

        modelBuilder.Entity<OrganizationProject>()
        .HasOne(e => e.Project)
        .WithMany(e => e.OrganizationProjects)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<OrganizationProject>()
        .HasOne(e => e.Organization)
        .WithMany(e => e.OrganizationProjects)
        .HasForeignKey(e => e.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<OrganizationAdmin>(entity =>
        {
            entity.ToTable("OrganizationAdmin");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.OrganizationId).HasColumnName("OrganizationId").IsRequired(true);
            entity.Property(table => table.AdminUserId).HasColumnName("AdminUserId").IsRequired(true);
        });

        modelBuilder.Entity<OrganizationAdmin>()
        .HasOne(e => e.Organization)
        .WithMany(e => e.OrganizationAdmins)
        .HasForeignKey(e => e.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<OrganizationAdmin>()
        .HasOne(e => e.AdminUser)
        .WithMany(e => e.OrganizationAdmins)
        .HasForeignKey(e => e.AdminUserId)
        .IsRequired(true);

        modelBuilder.Entity<OrganizationMember>(entity =>
        {
            entity.ToTable("OrganizationMember");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.OrganizationId).HasColumnName("OrganizationId").IsRequired(true);
            entity.Property(table => table.MemberUserId).HasColumnName("MemberUserId").IsRequired(true);
        });

        modelBuilder.Entity<OrganizationMember>()
        .HasOne(e => e.Organization)
        .WithMany(e => e.OrganizationMembers)
        .HasForeignKey(e => e.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<OrganizationMember>()
        .HasOne(e => e.MemberUser)
        .WithMany(e => e.OrganizationMembers)
        .HasForeignKey(e => e.MemberUserId)
        .IsRequired(true);

        modelBuilder.Entity<OrganizationAffiliate>(entity =>
        {
            entity.ToTable("OrganizationAffiliate");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.OrganizationId).HasColumnName("OrganizationId").IsRequired(true);
            entity.Property(table => table.AffiliateUserId).HasColumnName("AffiliateUserId").IsRequired(true);
        });

        modelBuilder.Entity<OrganizationAffiliate>()
        .HasOne(e => e.Organization)
        .WithMany(e => e.OrganizationAffiliates)
        .HasForeignKey(e => e.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<OrganizationAffiliate>()
        .HasOne(e => e.AffiliateUser)
        .WithMany(e => e.OrganizationAffiliates)
        .HasForeignKey(e => e.AffiliateUserId)
        .IsRequired(true);

        modelBuilder.Entity<ProjectUser>(entity =>
        {
            entity.ToTable("ProjectUser");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.ProjectId).HasColumnName("ProjectId").IsRequired(true);
            entity.Property(table => table.UserId).HasColumnName("UserId").IsRequired(true);
        });

        modelBuilder.Entity<ProjectUser>()
        .HasOne(e => e.Project)
        .WithMany(e => e.ProjectUsers)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<ProjectUser>()
        .HasOne(e => e.User)
        .WithMany(e => e.ProjectUsers)
        .HasForeignKey(e => e.UserId)
        .IsRequired(true);

        modelBuilder.Entity<ProjectAdmin>(entity =>
        {
            entity.ToTable("ProjectAdmin");
            entity.HasKey(table => table.Id);
            entity.Property(table => table.Id).HasColumnName("Id").IsRequired(true)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
            entity.Property(table => table.ProjectId).HasColumnName("ProjectId").IsRequired(true);
            entity.Property(table => table.AdminId).HasColumnName("UserId").IsRequired(true);
        });

        modelBuilder.Entity<ProjectAdmin>()
        .HasOne(e => e.Project)
        .WithMany(e => e.ProjectAdmins)
        .HasForeignKey(e => e.ProjectId)
        .IsRequired(true);

        modelBuilder.Entity<ProjectAdmin>()
        .HasOne(e => e.Admin)
        .WithMany(e => e.ProjectAdmins)
        .HasForeignKey(e => e.AdminId)
        .IsRequired(true);

        base.OnModelCreating(modelBuilder);
    }

    public void ConfigureDefault()
    {
        Configure(db =>
        {
            var isDesignTime = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_MIGRATION") == "true";

            if (!isDesignTime)
            {
                db.Migrate();
            }
        });
    }
}
