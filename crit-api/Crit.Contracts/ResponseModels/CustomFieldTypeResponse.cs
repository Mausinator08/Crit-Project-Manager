using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class CustomFieldTypeResponse
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid ProjectId { get; set; } = Guid.Empty;
    public Guid? HiddenProjectId { get; set; }

    public List<CustomFieldResponse> CustomFields { get; set; } = new List<CustomFieldResponse>();
}
