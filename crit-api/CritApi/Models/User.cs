using System.ComponentModel.DataAnnotations;

namespace CritDTO.Models;

public class User
{
    public User(string userName, string email, string? password = null, string? twoFactorCode = null, string? twoFactorRecoveryCode = null)
    {
        UserName = userName;
        Email = email;
        Password = password;
        TwoFactorCode = twoFactorCode;
        TwoFactorRecoveryCode = twoFactorRecoveryCode;
    }

    public User(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }

    [Required]
    public string? Password { get; set; }

    public string? TwoFactorCode { get; set; }

    public string? TwoFactorRecoveryCode { get; set; }
}
