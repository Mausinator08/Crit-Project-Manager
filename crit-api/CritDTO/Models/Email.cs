using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository.Attributes;

namespace CritDTO.Models;

public class Email
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    [ProtectedPersonalData]
    public string? EmailAddress { get; set; }
    public string OrganizationId { get; set; } = string.Empty;

    public Organization? Organization { get; set; }
}
