using System.Text.Json.Serialization;
using Crit.Contracts.Enums;

namespace Crit.Contracts.ResponseModels;

public class PhoneNumberResponse
{
    public PhoneNumberResponse()
    {
        Type = PhoneNumberType.Mobile;
        CountryCode = "+1";
        Number = "000-000-0000";
        OrganizationId = Guid.Empty;
    }

    public Guid? Id { get; set; }
    public PhoneNumberType Type { get; set; }
    public string CountryCode { get; set; }
    public string Number { get; set; }
    public string? Extension { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid? UserId { get; set; }
}
