using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class StatusResponse
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
    public Guid ProjectId { get; set; } = Guid.Empty;
    public Guid? TaskId { get; set; }
}
