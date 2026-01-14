
using Crit.Domain.Identity;
namespace Crit.Domain.Models;

public class MentionedUserComment
{
	public MentionedUserComment()
	{
		ApplicationUserId = Guid.Empty;
		CommentId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid ApplicationUserId { get; set; }
	public Guid CommentId { get; set; }

	public ApplicationUser? ApplicationUser { get; set; }
	public Comment? Comment { get; set; }
}
