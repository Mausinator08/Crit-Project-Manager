using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class CommentToCommentResponseMapper : IMapper<Comment, CommentResponse>
{
	private readonly IMapperService _mapperService;

	public CommentToCommentResponseMapper(IMapperService mapperService)
	{
		_mapperService = mapperService;
	}

	public CommentResponse ConvertTo(Comment fromModel)
	{
		return new CommentResponse()
		{
			Id = fromModel.Id,
			MentionedUsers = _mapperService.ConvertListTo<MentionedUserComment, MentionedUserCommentResponse>(fromModel.MentionedUsers),
			ProjectId = fromModel.ProjectId,
			QuotedComments = _mapperService.ConvertListTo<CommentQuote, CommentQuoteResponse>(fromModel.QuotedComments),
			TaskId = fromModel.TaskId,
			Text = fromModel.Text
		};
	}
}
