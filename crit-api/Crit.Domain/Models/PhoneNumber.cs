using Crit.Abstractions.Types;
using Microsoft.AspNetCore.Identity;
namespace Crit.Domain.Models;

public class PhoneNumber
{
    public PhoneNumber()
    {
        Type = PhoneNumberType.Mobile;
        CountryCode = "+1";
        Number = "000-000-0000";
        OrganizationId = Guid.Empty;
    }

    public Guid? Id { get; set; }
    public PhoneNumberType Type { get; set; }
    [PersonalData]
    public string? CountryCode { get; set; }
    [PersonalData]
    public string? Number { get; set; }
    [PersonalData]
    public string? Extension { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid? UserId { get; set; }

    public Organization? Organization { get; set; }
}
