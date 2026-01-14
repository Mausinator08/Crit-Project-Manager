namespace Crit.Contracts.RequestModels;

public class CreateCustomFieldTypeRequest
{
	public string Name { get; set; } = string.Empty;
	public Guid ProjectId { get; set; } = Guid.Empty;
	public Guid? HiddenProjectId { get; set; }
}
