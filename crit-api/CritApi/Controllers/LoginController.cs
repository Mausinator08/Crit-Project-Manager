using System.Text;
using System.Text.Json.Nodes;
using CritApi.Models;
using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Services;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api")]
public class LoginController : ControllerBase
{
    private readonly Logging.ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserRepository _userRepository;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly IPhoneNumberRepository _phoneNumberRepository;
    public LoginController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        Logging.ILogger logger,
        IUserRepository userRepository,
        IOrganizationRepository organizationRepository,
        IEmailRepository emailRepository,
        IPhoneNumberRepository phoneNumberRepository)
    {
        _userRepository = userRepository;
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _organizationRepository = organizationRepository;
        _emailRepository = emailRepository;
        _phoneNumberRepository = phoneNumberRepository;
    }

    [HttpPost]
    [Route("Register")]
    [ProducesResponseType<object>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        try
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            if (user.Password == null)
            {
                return BadRequest("Password is required.");
            }

            ApplicationUser appUser = new ApplicationUser() { UserName = user.UserName, Email = user.Email };

            if (appUser.UserName != null && await _userManager.FindByNameAsync(appUser.UserName) != null)
            {
                return BadRequest("User already exists.");
            }

            if (!_userManager.Users.Any() && !(await _userManager.GetUsersInRoleAsync("SuperAdmin")).Any())
            {
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (!result.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create user.\n" + stringBuilder.ToString());
                }

                if (string.IsNullOrWhiteSpace(user.Organization))
                {
                    return BadRequest("Organization name is required.");
                }

                Organization organization = await _organizationRepository.CreateOrganization(new Organization(user.Organization, appUser.Id));

                if (organization == null || string.IsNullOrWhiteSpace(organization.Id))
                {
                    throw new Exception("Failed to create organization.");
                }

                Email email = await _emailRepository.CreateEmail(new Email()
                {
                    EmailAddress = user.Email,
                    OrganizationId = organization.Id,
                    Organization = organization,
                    UserId = appUser.Id
                });

                PhoneNumber phoneNumber = await _phoneNumberRepository.CreatePhoneNumber(new PhoneNumber()
                {
                    Number = user.PhoneNumber ?? string.Empty,
                    OrganizationId = organization.Id,
                    Organization = organization,
                    CountryCode = user.CountryCode ?? "+1",
                    Extension = user.Extension,
                    Type = user.PhoneType.HasValue ? user.PhoneType.Value : PhoneNumberType.Mobile
                });

                if (string.IsNullOrWhiteSpace(user.Organization))
                {
                    return BadRequest("Organization name is required.");
                }

                IdentityResult createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "SuperAdmin" });

                if (!createRoleResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create SuperAdmin role.");
                }

                createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "OrganizationOwner" });

                if (!createRoleResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create OrganizationOwner role.");
                }

                createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "OrganizationAdmin" });

                if (!createRoleResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create OrganizationAdmin role.");
                }

                createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "ProjectOwner" });

                if (!createRoleResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create ProjectOwner role.");
                }

                createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "ProjectAdmin" });

                if (!createRoleResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create ProjectAdmin role.");
                }

                createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "User" });

                if (!createRoleResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create User role.");
                }

                IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

                if (!roleResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create user with SuperAdmin role.");
                }

                roleResult = await _userManager.AddToRoleAsync(appUser, "OrganizationOwner");

                if (!roleResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create user with OrganizationOwner role.");
                }
            }
            else
            {
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (!result.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (IdentityError error in result.Errors)
                    {
                        stringBuilder.AppendLine(error.Description);
                    }

                    throw new Exception("Failed to create user.\n" + stringBuilder.ToString());
                }

                if (string.IsNullOrWhiteSpace(user.Organization))
                {
                    return BadRequest("Organization name is required.");
                }

                Organization? organization = null;
                IEnumerable<Organization> organizationQuery = (await _organizationRepository.GetAllOrganizations()).Where(o => o.Name == user.Organization);

                if (!organizationQuery.Any())
                {
                    organization = await _organizationRepository.CreateOrganization(new Organization(user.Organization, appUser.Id));

                    IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "OrganizationOwner");

                    if (!roleResult.Succeeded)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (IdentityError error in result.Errors)
                        {
                            stringBuilder.AppendLine(error.Description);
                        }

                        throw new Exception("Failed to create user with User role.");
                    }
                }
                else
                {
                    organization = organizationQuery.First();

                    IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if (!roleResult.Succeeded)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (IdentityError error in result.Errors)
                        {
                            stringBuilder.AppendLine(error.Description);
                        }

                        throw new Exception("Failed to create user with User role.");
                    }
                }

                if (organization == null || string.IsNullOrWhiteSpace(organization.Id))
                {
                    throw new Exception("Failed to get or create organization.");
                }

                Email email = await _emailRepository.CreateEmail(new Email()
                {
                    EmailAddress = user.Email,
                    OrganizationId = organization.Id,
                    Organization = organization,
                    UserId = appUser.Id
                });

                PhoneNumber phoneNumber = await _phoneNumberRepository.CreatePhoneNumber(new PhoneNumber()
                {
                    Number = user.PhoneNumber ?? string.Empty,
                    OrganizationId = organization.Id,
                    Organization = organization,
                    CountryCode = user.CountryCode ?? "+1",
                    Extension = user.Extension,
                    Type = user.PhoneType.HasValue ? user.PhoneType.Value : PhoneNumberType.Mobile
                });
            }

            return Ok(new { userName = appUser.UserName, message = $"Welcome to Crit! Enjoy your stay, {appUser.UserName}!" });
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error registering user.");
        }
    }

    [HttpPost]
    [Route("Login")]
    [ProducesResponseType<object>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> Login([FromBody] User user, [FromQuery] bool? useCookies)
    {
        try
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            bool isPersistent = useCookies == true;

            // Simulate user authentication
            if (string.IsNullOrEmpty(user.UserName))
            {
                return BadRequest("Username is required.");
            }

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

                return Ok(new { userName = applicationUser.UserName, message = "You have logged in." });
            }
            else
            {
                return Unauthorized("Invalid credentials.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error logging in.");
        }
    }

    [HttpGet]
    [Route("IsAuthenticated")]
    [ProducesResponseType<object>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> IsAuthenticated()
    {
        try
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();

                if (applicationUser == null)
                {
                    await _signInManager.SignOutAsync().ConfigureAwait(false);
                    return Ok(new { message = "User is not logged in. Returning to login.", authenticated = false });
                }

                return Ok(new { message = $"Welcome to Crit, {applicationUser.UserName}!", authenticated = true });
            }
            else
            {
                return Ok(new { message = "User is not authenticated. Please login.", authenticated = false });
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error checking authentication.");
        }
    }

    [HttpPost]
    [Route("Logout")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();

                if (applicationUser == null)
                {
                    return Unauthorized("User is not logged in.");
                }

                await _signInManager.SignOutAsync().ConfigureAwait(false);
                return Ok("You have logged out successfully.");
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