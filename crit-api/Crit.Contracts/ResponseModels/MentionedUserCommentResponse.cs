using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class MentionedUserCommentResponse
{
	public MentionedUserCommentResponse()
	{
		ApplicationUserId = Guid.Empty;
		CommentId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid ApplicationUserId { get; set; }
	public Guid CommentId { get; set; }
}
