namespace CritDTO.Models;

public class CustomField
{
    public CustomField(string name, object value, Guid type, Guid taskId)
    {
        CustomFieldTypeId = type;
        Name = name;
        Value = value;
        TaskId = taskId;
    }

    private CustomField()
    {
        Name = "";
        Value = 0;
    }

    public Guid Id { get; set; }
    public Guid CustomFieldTypeId { get; set; }
    public string Name { get; set; }
    public object Value { get; set; }
    public Guid TaskId { get; set; }

    public Task? Task { get; set; }
    public CustomFieldType? CustomFieldType { get; set; }
}
