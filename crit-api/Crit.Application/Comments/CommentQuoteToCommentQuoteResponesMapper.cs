using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class CommentQuoteToCommentQuoteResponesMapper : IMapper<CommentQuote, CommentQuoteResponse>
{
	public CommentQuoteResponse ConvertTo(CommentQuote fromModel)
	{
		return new CommentQuoteResponse()
		{
			Id = fromModel.Id,
			CommentId = fromModel.CommentId,
			QuotedCommentId = fromModel.QuotedCommentId
		};
	}
}
