namespace Crit.Contracts.RequestModels;

public class CreateProjectTaskRequest
{
	public string? Title { get; set; }
	public string? Details { get; set; }
	public Guid ProjectId { get; set; } = Guid.Empty;
	public Guid? AssignedUserId { get; set; }
	public Guid? StatusId { get; set; }
	public Guid? PriorityId { get; set; }
	public int? Complexity { get; set; }
	public DateTime? DueDate { get; set; }
	public Guid? ParentTaskId { get; set; }
}
