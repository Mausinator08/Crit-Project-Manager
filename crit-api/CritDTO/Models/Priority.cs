namespace CritDTO.Models;

public class Priority
{
    public Priority(string name, string? backgroundColor, string? color, Guid projectId)
    {
        Name = name;
        BackgroundColor = backgroundColor;
        Color = color;
        ProjectId = projectId;
        Tasks = new List<ProjectTask>();
    }

    private Priority()
    {
        Name = "";
        Tasks = new List<ProjectTask>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
    public Guid ProjectId { get; set; }
    public Guid TaskId { get; set; }

    public Project? Project { get; set; }
    public List<ProjectTask> Tasks { get; set; }
}