using System.Text.Json.Serialization;

namespace CritDTO.Models;

public class Priority
{
    [JsonConstructor]
    public Priority(string name, string? backgroundColor, string? color, Guid projectId)
    {
        Name = name;
        BackgroundColor = backgroundColor;
        Color = color;
        ProjectId = projectId;
        Tasks = new List<ProjectTask>();
    }

    protected Priority()
    {
        Name = string.Empty;
        Tasks = new List<ProjectTask>();
        ProjectId = Guid.Empty;
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? TaskId { get; set; }

    [JsonIgnore]
    public virtual Project? Project { get; set; }
    [JsonIgnore]
    public virtual List<ProjectTask> Tasks { get; set; }
}