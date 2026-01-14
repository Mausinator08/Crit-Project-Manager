namespace Crit.Domain.Models;

public class Status
{
    protected Status()
    {
        Name = string.Empty;
        Tasks = new List<ProjectTask>();
        ProjectId = Guid.Empty;
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? TaskId { get; set; }

    public Project? Project { get; set; }
    public List<ProjectTask> Tasks { get; set; }
}
