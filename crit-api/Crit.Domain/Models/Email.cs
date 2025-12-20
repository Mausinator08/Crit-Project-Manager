using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Crit.Domain.Models;

public class Email
{
    public Guid? Id { get; set; }
    public Guid? UserId { get; set; }
    [ProtectedPersonalData]
    public string? EmailAddress { get; set; }
    public Guid OrganizationId { get; set; } = Guid.Empty;

    [JsonIgnore]
    public Organization? Organization { get; set; }
}
