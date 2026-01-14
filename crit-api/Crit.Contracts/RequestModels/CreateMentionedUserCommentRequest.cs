namespace Crit.Contracts.RequestModels;

public class CreateMentionedUserCommentRequest
{
	public Guid ApplicationUserId { get; set; } = Guid.Empty;
	public Guid CommentId { get; set; } = Guid.Empty;
}
