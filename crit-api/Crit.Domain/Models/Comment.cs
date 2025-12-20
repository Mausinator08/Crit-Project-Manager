using System.Text.Json.Serialization;
using Crit.Domain.BaseModels;

namespace Crit.Domain.Models;

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

	public Guid? Id { get; set; }
	public string? Text { get; set; }
	public Guid? TaskId { get; set; }
	public Guid ProjectId { get; set; }

	public ProjectTask? Task { get; set; }
	public Project? Project { get; set; }
	public List<CommentQuote> QuotedComments { get; set; }
	public List<MentionedUserComment> MentionedUsers { get; set; }
}
