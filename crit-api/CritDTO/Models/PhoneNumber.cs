using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository.Attributes;

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
        OrganizationId = string.Empty;
    }

    public string? Id { get; set; }
    public PhoneNumberType Type { get; set; }
    [PersonalData]
    public string CountryCode { get; set; }
    [PersonalData]
    public string Number { get; set; }
    [PersonalData]
    public string? Extension { get; set; }
    public string OrganizationId { get; set; }
    public string? UserId { get; set; }

    public Organization? Organization { get; set; }
}
