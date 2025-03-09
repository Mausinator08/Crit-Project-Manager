namespace CritDTO.Models;

public class CustomFieldType
{
    public CustomFieldType(string name, Guid projectId)
    {
        Name = name;
        ProjectId = projectId;
        CustomFields = new List<CustomField>();
    }

    private CustomFieldType()
    {
        Name = "";
        CustomFields = new List<CustomField>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ProjectId { get; set; }

    public Project? Project { get; set; }
    public List<CustomField> CustomFields { get; set; }
}
