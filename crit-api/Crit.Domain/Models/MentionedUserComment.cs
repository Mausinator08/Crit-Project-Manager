using System.Text.Json.Serialization;
using Crit.Domain.Identity;

namespace Crit.Domain.Entities;

public class MentionedUserComment
{
	public MentionedUserComment()
	{
		ApplicationUserId = Guid.Empty;
		CommentId = Guid.Empty;
	}

	[JsonConstructor]
	public MentionedUserComment(Guid applicationUserId, Guid commentId)
	{
		ApplicationUserId = applicationUserId;
		CommentId = commentId;
	}

	public Guid? Id { get; set; }
	public Guid ApplicationUserId { get; set; }
	public Guid CommentId { get; set; }

	[JsonIgnore]
	public virtual ApplicationUser? ApplicationUser { get; set; }
	[JsonIgnore]
	public virtual Comment? Comment { get; set; }
}
