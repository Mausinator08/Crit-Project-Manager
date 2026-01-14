using System.ComponentModel.DataAnnotations;
using Crit.Abstractions.Types;

namespace Crit.Contracts.RequestModels;

public class CreateUserRequest
{
    public Guid? Id { get; set; }
    public string UserName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? TwoFactorCode { get; set; }

    public string? TwoFactorRecoveryCode { get; set; }
    public string? Organization { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CountryCode { get; set; }
    public string? Extension { get; set; }
    public PhoneNumberType PhoneType { get; set; }
}
