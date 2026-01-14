namespace Crit.Domain.Models;

public class CommentQuote
{
	public CommentQuote()
	{
		CommentId = Guid.Empty;
		QuotedCommentId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid CommentId { get; set; }
	public Guid QuotedCommentId { get; set; }

	public Comment? Comment { get; set; }
	public Comment? QuotedComment { get; set; }
}
