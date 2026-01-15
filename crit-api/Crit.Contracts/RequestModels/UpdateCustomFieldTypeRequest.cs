namespace Crit.Contracts.RequestModels;

public class UpdateCustomFieldTypeRequest
{
	public string? Name { get; set; }
	public Guid? ProjectId { get; set; }
	public Guid? HiddenProjectId { get; set; }
}
