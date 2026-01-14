namespace Crit.Contracts.RequestModels;

public class CreateCommentQuoteRequest
{
	public Guid CommentId { get; set; } = Guid.Empty;
	public Guid QuotedCommentId { get; set; } = Guid.Empty;
}
