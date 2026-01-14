namespace Crit.Contracts.RequestModels;

public class CreateCustomFieldRequest
{
	public Guid CustomFieldTypeId { get; set; } = Guid.Empty;
	public string Value { get; set; } = string.Empty;
	public Guid TaskId { get; set; } = Guid.Empty;
}
