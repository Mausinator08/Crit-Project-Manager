namespace Crit.Domain.Models;

public class CustomField
{
    public CustomField(string name, string value, Guid typeId, Guid taskId)
    {
        CustomFieldTypeId = typeId;
        Value = value;
        TaskId = taskId;
    }

    protected CustomField()
    {
        Value = string.Empty;
        CustomFieldTypeId = Guid.Empty;
        TaskId = Guid.Empty;
    }

    public Guid? Id { get; set; }
    public Guid CustomFieldTypeId { get; set; }
    public string Value { get; set; }
    public Guid TaskId { get; set; }

    public ProjectTask? Task { get; set; }
    public CustomFieldType? CustomFieldType { get; set; }
}
