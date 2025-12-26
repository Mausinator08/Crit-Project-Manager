using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class CommentQuoteResponse
{
	public CommentQuoteResponse()
	{
		CommentId = Guid.Empty;
		QuotedCommentId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid CommentId { get; set; }
	public Guid QuotedCommentId { get; set; }
}
