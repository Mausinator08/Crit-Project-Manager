using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class CustomFieldResponse
{
    public CustomFieldResponse(string name, string value, Guid typeId, Guid taskId)
    {
        CustomFieldTypeId = typeId;
        Value = value;
        TaskId = taskId;
    }

    protected CustomFieldResponse()
    {
        Value = string.Empty;
        CustomFieldTypeId = Guid.Empty;
        TaskId = Guid.Empty;
    }

    public Guid? Id { get; set; }
    public Guid CustomFieldTypeId { get; set; }
    public string Value { get; set; }
    public Guid TaskId { get; set; }
}
