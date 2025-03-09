namespace CritDTO.Models;

public class Status
{
    public Status(string name, string? description, string? backgroundColor, string? color, Guid projectId)
    {
        Name = name;
        Description = description;
        BackgroundColor = backgroundColor;
        Color = color;
        ProjectId = projectId;
        Tasks = new List<Task>();
    }

    private Status()
    {
        Name = "";
        Tasks = new List<Task>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
    public Guid ProjectId { get; set; }
    public Guid TaskId { get; set; }

    public Project? Project { get; set; }
    public List<Task> Tasks { get; set; }
}
