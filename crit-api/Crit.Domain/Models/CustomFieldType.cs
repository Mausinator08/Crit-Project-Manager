namespace Crit.Domain.Models;

public class CustomFieldType
{
    public CustomFieldType(string name, Guid projectId)
    {
        Name = name;
        ProjectId = projectId;
        CustomFields = new List<CustomField>();
    }

    protected CustomFieldType()
    {
        Name = string.Empty;
        ProjectId = Guid.Empty;
        CustomFields = new List<CustomField>();
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? HiddenProjectId { get; set; }

    public Project? Project { get; set; }
    public Project? HiddenProject { get; set; }
    public List<CustomField> CustomFields { get; set; }
}
