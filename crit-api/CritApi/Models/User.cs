using System.ComponentModel.DataAnnotations;

namespace CritDTO.Models;

public class User
{
    public User(string userName, string email, string password)
    {
        UserName = userName;
        Email = email;
        Password = password;
    }

    public User(string userName, string email)
    {
        UserName = userName;
        Email = email;
        Password = null;
    }

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
