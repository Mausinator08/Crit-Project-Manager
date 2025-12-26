using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class PriorityResponse
{
    protected PriorityResponse()
    {
        Name = string.Empty;
        ProjectId = Guid.Empty;
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? TaskId { get; set; }
}