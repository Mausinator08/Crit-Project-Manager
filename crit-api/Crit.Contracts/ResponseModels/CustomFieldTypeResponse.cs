using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class CustomFieldTypeResponse
{
    public CustomFieldTypeResponse(string name, Guid projectId)
    {
        Name = name;
        ProjectId = projectId;
        CustomFields = new List<CustomFieldResponse>();
    }

    protected CustomFieldTypeResponse()
    {
        Name = string.Empty;
        ProjectId = Guid.Empty;
        CustomFields = new List<CustomFieldResponse>();
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? HiddenProjectId { get; set; }

    public List<CustomFieldResponse> CustomFields { get; set; }
}
