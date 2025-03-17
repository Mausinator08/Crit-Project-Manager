namespace CritDTO.Models;

public class CustomField
{
    public CustomField(string name, string value, string type, string taskId)
    {
        CustomFieldTypeId = type;
        Name = name;
        Value = value;
        TaskId = taskId;
    }

    private CustomField()
    {
        Name = string.Empty;
        Value = string.Empty;
        CustomFieldTypeId = string.Empty;
        TaskId = string.Empty;
    }

    public string? Id { get; set; }
    public string CustomFieldTypeId { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string TaskId { get; set; }

    public ProjectTask? Task { get; set; }
    public CustomFieldType? CustomFieldType { get; set; }
}
