using Crit.Domain.Modules.ProjectManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crit.Application.Modules.ProjectManagement.Contracts.Persistence;

public interface ICritProjectManagementContext
{
	DbSet<Project> Projects { get; set; }
	DbSet<Status> Statuses { get; set; }
	DbSet<Priority> Priorities { get; set; }
	DbSet<CustomFieldType> CustomFieldTypes { get; set; }
	DbSet<CustomField> CustomFields { get; set; }
	DbSet<ProjectTask> Tasks { get; set; }
	DbSet<Comment> Comments { get; set; }
	DbSet<CommentQuote> CommentQuotes { get; set; }
	DbSet<MentionedUserComment> MentionedUserComments { get; set; }
}
