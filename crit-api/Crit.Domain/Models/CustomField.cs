using System.Text.Json.Serialization;

namespace Crit.Domain.Entities;

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

    [JsonIgnore]
    public virtual ProjectTask? Task { get; set; }
    [JsonIgnore]
    public virtual CustomFieldType? CustomFieldType { get; set; }
}
