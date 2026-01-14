using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class ProjectTaskResponse
{
    public ProjectTaskResponse()
    {
        ProjectId = Guid.Empty;
        SubTasks = new List<ProjectTaskResponse>();
        CustomFields = new List<CustomFieldResponse>();
        Comments = new List<CommentResponse>();
    }

    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? AssignedUserId { get; set; }
    public Guid? StatusId { get; set; }
    public Guid? PriorityId { get; set; }
    public int? Complexity { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ParentTaskId { get; set; }

    public List<CustomFieldResponse> CustomFields { get; set; }
    public StatusResponse? Status { get; set; }
    public PriorityResponse? Priority { get; set; }
    public List<ProjectTaskResponse> SubTasks { get; set; }
    public List<CommentResponse> Comments { get; set; }
}
