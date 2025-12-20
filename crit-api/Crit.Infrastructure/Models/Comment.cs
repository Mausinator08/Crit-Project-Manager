using System.Text.Json.Serialization;
using Crit.Contracts.Models.BaseModels;

namespace Crit.Contracts.Models;

public class Comment : AuditInformation
{
	public Comment()
	{
		Text = string.Empty;
		TaskId = null;
		ProjectId = Guid.Empty;
		QuotedComments = new List<CommentQuote>();
		MentionedUsers = new List<MentionedUserComment>();
		DateTime now = DateTime.UtcNow;
		DateCreated = now;
		DateUpdated = now;
	}

	[JsonConstructor]
	public Comment(Guid projectId, Guid createdByUserId, Guid? taskId = null, string text = "")
	{
		Text = text;
		TaskId = taskId;
		ProjectId = projectId;
		QuotedComments = new List<CommentQuote>();
		MentionedUsers = new List<MentionedUserComment>();
		DateTime now = DateTime.UtcNow;
		DateCreated = now;
		DateUpdated = now;
		CreatedByUserId = createdByUserId;
		UpdatedByUserId = createdByUserId;
	}

	public Guid? Id { get; set; }
	public string? Text { get; set; }
	public Guid? TaskId { get; set; }
	public Guid ProjectId { get; set; }

	[JsonIgnore]
	public virtual ProjectTask? Task { get; set; }
	[JsonIgnore]
	public virtual Project? Project { get; set; }
	[JsonIgnore]
	public virtual List<CommentQuote> QuotedComments { get; set; }
	[JsonIgnore]
	public virtual List<MentionedUserComment> MentionedUsers { get; set; }
}
