using System.Text.Json.Nodes;
using CritDataAccess.Services;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly Logging.ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, Logging.ILogger logger)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    [Route("Login")]
    [ProducesResponseType<object>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> Login([FromBody] User user, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
    {
        try
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            bool useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            bool isPersistent = (useCookies == true) && (useSessionCookies != true);

            _signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

            // Simulate user authentication
            ApplicationUser? applicationUser = await _userManager.FindByNameAsync(user.UserName);
            if (applicationUser != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(applicationUser, user.Password ?? string.Empty, isPersistent, false);

                if (result.RequiresTwoFactor)
                {
                    if (!string.IsNullOrWhiteSpace(user.TwoFactorCode))
                    {
                        result = await _signInManager.TwoFactorAuthenticatorSignInAsync(user.TwoFactorCode, isPersistent, isPersistent);
                    }
                    else if (!string.IsNullOrWhiteSpace(user.TwoFactorRecoveryCode))
                    {
                        result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(user.TwoFactorRecoveryCode);
                    }
                }

                if (!result.Succeeded)
                {
                    return Unauthorized(result.ToString());
                }

                return Ok(new { userName = applicationUser.UserName, firstUser = false, message = "User logged in." });
            }
            else
            {
                if (user.Password != null)
                {
                    ApplicationUser appUser = new ApplicationUser() { UserName = user.UserName, Email = user.Email };
                    IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "OrganizationOwner");
                    return Ok(new { userName = appUser.UserName, firstUser = true, message = "Organization owner created." });
                }
                else
                {
                    return Unauthorized("Invalid credentials.");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error logging in.");
        }
    }

    [HttpGet]
    [Route("Logout")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] object empty)
    {
        try
        {
            if (empty != null)
            {
                await _signInManager.SignOutAsync().ConfigureAwait(false);
                return Ok();
            }

            return Unauthorized("User is not logged in.");
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error logging in.");
        }
    }
}