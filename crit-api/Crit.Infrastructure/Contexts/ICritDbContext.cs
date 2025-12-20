using Crit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Crit.Domain.Contexts;

public interface ICritDbContext
{
    DbSet<Organization> Organizations { get; set; }
    DbSet<Email> Emails { get; set; }
    DbSet<PhoneNumber> PhoneNumbers { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<Status> Statuses { get; set; }
    DbSet<Priority> Priorities { get; set; }
    DbSet<CustomFieldType> CustomFieldTypes { get; set; }
    DbSet<CustomField> CustomFields { get; set; }
    DbSet<ProjectTask> Tasks { get; set; }
    DbSet<Comment> Comments { get; set; }
    DbSet<CommentQuote> CommentQuotes { get; set; }
    DbSet<MentionedUserComment> MentionedUserComments { get; set; }
    DbSet<OrganizationProject> OrganizationProjects { get; set; }
    DbSet<OrganizationAdmin> OrganizationAdmins { get; set; }
    DbSet<OrganizationMember> OrganizationMembers { get; set; }
    DbSet<OrganizationAffiliate> OrganizationAffiliates { get; set; }
    DbSet<ProjectUser> ProjectUsers { get; set; }
    DbSet<ProjectAdmin> ProjectAdmins { get; set; }

    void Configure(Action<DatabaseFacade> databaseAction);
}
