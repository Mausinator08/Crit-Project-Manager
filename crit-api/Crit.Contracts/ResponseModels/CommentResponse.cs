using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class CommentResponse : AuditInformationResponse
{
	public CommentResponse()
	{
		Text = string.Empty;
		TaskId = null;
		ProjectId = Guid.Empty;
		QuotedComments = new List<CommentQuoteResponse>();
		MentionedUsers = new List<MentionedUserCommentResponse>();
		DateTime now = DateTime.UtcNow;
		DateCreated = now;
		DateUpdated = now;
	}

	public Guid? Id { get; set; }
	public string? Text { get; set; }
	public Guid? TaskId { get; set; }
	public Guid ProjectId { get; set; }

	public List<CommentQuoteResponse> QuotedComments { get; set; }
	public List<MentionedUserCommentResponse> MentionedUsers { get; set; }
}
