using System.Text.Json.Serialization;

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

    [JsonIgnore]
    public Project? Project { get; set; }
    [JsonIgnore]
    public Project? HiddenProject { get; set; }
    [JsonIgnore]
    public List<CustomField> CustomFields { get; set; }
}
