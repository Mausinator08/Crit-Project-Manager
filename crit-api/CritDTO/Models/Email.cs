using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository.Attributes;

namespace CritDTO.Models;

public class Email
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    [ProtectedPersonalData]
    public string? EmailAddress { get; set; }
    public Guid OrganizationId { get; set; }

    public Organization? Organization { get; set; }
}
