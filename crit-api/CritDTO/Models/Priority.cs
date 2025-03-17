namespace CritDTO.Models;

public class Priority
{
    public Priority(string name, string? backgroundColor, string? color, string projectId)
    {
        Name = name;
        BackgroundColor = backgroundColor;
        Color = color;
        ProjectId = projectId;
        Tasks = new List<ProjectTask>();
    }

    private Priority()
    {
        Name = string.Empty;
        Tasks = new List<ProjectTask>();
        ProjectId = string.Empty;
    }

    public string? Id { get; set; }
    public string Name { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
    public string ProjectId { get; set; }
    public string? TaskId { get; set; }

    public Project? Project { get; set; }
    public List<ProjectTask> Tasks { get; set; }
}