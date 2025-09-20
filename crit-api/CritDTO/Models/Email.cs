using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace CritDTO.Models;

public class Email
{
    public Guid? Id { get; set; }
    public Guid? UserId { get; set; }
    [ProtectedPersonalData]
    public string? EmailAddress { get; set; }
    public Guid OrganizationId { get; set; } = Guid.Empty;

    [JsonIgnore]
    public virtual Organization? Organization { get; set; }
}
