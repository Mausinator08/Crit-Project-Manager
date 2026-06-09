namespace Crit.Domain.Modules.ProjectManagement.Entities;

public class Priority
{
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

    public Project? Project { get; set; }
    public List<ProjectTask> Tasks { get; set; }
}