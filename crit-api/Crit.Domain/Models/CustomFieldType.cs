using System.Text.Json.Serialization;

namespace Crit.Domain.Entities;

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
    public virtual Project? Project { get; set; }
    [JsonIgnore]
    public virtual Project? HiddenProject { get; set; }
    [JsonIgnore]
    public virtual List<CustomField> CustomFields { get; set; }
}
