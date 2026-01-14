using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class CommentResponse
{
	public CommentResponse()
	{
		Text = string.Empty;
		TaskId = null;
		ProjectId = Guid.Empty;
		QuotedComments = new List<CommentQuoteResponse>();
		MentionedUsers = new List<MentionedUserCommentResponse>();
	}

	public Guid? Id { get; set; }
	public string? Text { get; set; }
	public Guid? TaskId { get; set; }
	public Guid ProjectId { get; set; }

	public List<CommentQuoteResponse> QuotedComments { get; set; }
	public List<MentionedUserCommentResponse> MentionedUsers { get; set; }
}
