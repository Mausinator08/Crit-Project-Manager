namespace Crit.Domain.Modules.ProjectManagement.Entities;

public class CustomFieldType
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid ProjectId { get; set; } = Guid.Empty;
    public Guid? HiddenProjectId { get; set; }

    public Project? Project { get; set; }
    public Project? HiddenProject { get; set; }
    public List<CustomField> CustomFields { get; set; } = new List<CustomField>();
}
