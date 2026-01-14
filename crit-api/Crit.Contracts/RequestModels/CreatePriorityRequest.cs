namespace Crit.Contracts.RequestModels;

public class CreatePriorityRequest
{
	public string Name { get; set; } = string.Empty;
	public string? BackgroundColor { get; set; }
	public string? Color { get; set; }
	public Guid ProjectId { get; set; } = Guid.Empty;
	public Guid? TaskId { get; set; }
}
