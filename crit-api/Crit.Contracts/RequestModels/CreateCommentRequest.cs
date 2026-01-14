namespace Crit.Contracts.RequestModels;

public class CreateCommentRequest
{
	public string? Text { get; set; }
	public Guid? TaskId { get; set; }
	public Guid ProjectId { get; set; } = Guid.Empty;
}
