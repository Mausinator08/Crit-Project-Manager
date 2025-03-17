namespace CritDTO.Models;

public class Status
{
    public Status(string name, string? description, string? backgroundColor, string? color, string projectId)
    {
        Name = name;
        Description = description;
        BackgroundColor = backgroundColor;
        Color = color;
        ProjectId = projectId;
        Tasks = new List<ProjectTask>();
    }

    private Status()
    {
        Name = string.Empty;
        Tasks = new List<ProjectTask>();
        ProjectId = string.Empty;
    }

    public string? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
    public string ProjectId { get; set; }
    public string? TaskId { get; set; }

    public Project? Project { get; set; }
    public List<ProjectTask> Tasks { get; set; }
}
