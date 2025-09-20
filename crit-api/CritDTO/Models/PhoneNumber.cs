using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace CritDTO.Models;

public enum PhoneNumberType
{
    OrganizationPrimary,
    OrganizationSupport,
    OrganizationOther,
    OrganizationOwner,
    OrganizationAdmin,
    ProjectOwner,
    ProjectAdmin,
    User,
    Mobile,
    Office,
    Work,
    Personal,
    Fax,
    Landline
}

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
    public string CountryCode { get; set; }
    [PersonalData]
    public string Number { get; set; }
    [PersonalData]
    public string? Extension { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid? UserId { get; set; }

    [JsonIgnore]
    public virtual Organization? Organization { get; set; }
}
