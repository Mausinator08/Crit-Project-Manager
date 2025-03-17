namespace CritDTO.Models;

public class CustomFieldType
{
    public CustomFieldType(string name, string projectId)
    {
        Name = name;
        ProjectId = projectId;
        CustomFields = new List<CustomField>();
    }

    private CustomFieldType()
    {
        Name = string.Empty;
        ProjectId = string.Empty;
        CustomFields = new List<CustomField>();
    }

    public string? Id { get; set; }
    public string Name { get; set; }
    public string ProjectId { get; set; }

    public Project? Project { get; set; }
    public List<CustomField> CustomFields { get; set; }
}
