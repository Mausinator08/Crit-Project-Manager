namespace CritDTO.Models;

public class CustomField
{
    public CustomField(string name, string value, Guid type, Guid taskId)
    {
        CustomFieldTypeId = type;
        Name = name;
        Value = value;
        TaskId = taskId;
    }

    private CustomField()
    {
        Name = "";
        Value = string.Empty;
    }

    public Guid Id { get; set; }
    public Guid CustomFieldTypeId { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public Guid TaskId { get; set; }

    public ProjectTask? Task { get; set; }
    public CustomFieldType? CustomFieldType { get; set; }
}
