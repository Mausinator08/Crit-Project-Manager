using System.Text.Json.Serialization;

namespace Crit.Domain.Entities;

public class CommentQuote
{
	public CommentQuote()
	{
		CommentId = Guid.Empty;
		QuotedCommentId = Guid.Empty;
	}

	[JsonConstructor]
	public CommentQuote(Guid commentId, Guid quotedCommentId)
	{
		CommentId = commentId;
		QuotedCommentId = quotedCommentId;
	}

	public Guid? Id { get; set; }
	public Guid CommentId { get; set; }
	public Guid QuotedCommentId { get; set; }

	[JsonIgnore]
	public virtual Comment? Comment { get; set; }
	[JsonIgnore]
	public virtual Comment? QuotedComment { get; set; }
}
