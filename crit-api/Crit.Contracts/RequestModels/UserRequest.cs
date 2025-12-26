using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Crit.Contracts.Enums;

namespace Crit.Contracts.RequestModels;

public class UserRequest
{
    [JsonConstructor]
    public UserRequest(string userName, string email, string? password = null, string? twoFactorCode = null, string? twoFactorRecoveryCode = null)
    {
        UserName = userName;
        Email = email;
        Password = password;
        TwoFactorCode = twoFactorCode;
        TwoFactorRecoveryCode = twoFactorRecoveryCode;
    }

    public UserRequest(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }

    public Guid? Id { get; set; }
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? TwoFactorCode { get; set; }

    public string? TwoFactorRecoveryCode { get; set; }
    public string? Organization { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CountryCode { get; set; }
    public string? Extension { get; set; }
    public PhoneNumberType? PhoneType { get; set; }
}
