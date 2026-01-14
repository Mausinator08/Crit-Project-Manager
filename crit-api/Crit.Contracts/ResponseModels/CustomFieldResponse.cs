using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class CustomFieldResponse
{
    public Guid? Id { get; set; }
    public Guid CustomFieldTypeId { get; set; } = Guid.Empty;
    public string Value { get; set; } = string.Empty;
    public Guid TaskId { get; set; } = Guid.Empty;
}
