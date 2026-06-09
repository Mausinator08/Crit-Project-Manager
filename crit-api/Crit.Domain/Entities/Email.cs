using Crit.Domain.Modules.OrganizationManagement.Entities;
using Microsoft.AspNetCore.Identity;

namespace Crit.Domain.Entities;

public class Email
{
    public Guid? Id { get; set; }
    public Guid? UserId { get; set; }
    [ProtectedPersonalData]
    public string? EmailAddress { get; set; }
    public Guid OrganizationId { get; set; } = Guid.Empty;

    public Organization? Organization { get; set; }
}
