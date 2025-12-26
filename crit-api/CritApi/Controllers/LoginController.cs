using System.Text;
using Crit.Application.Logins;
using Crit.Application.RepositoryInterfaces;
using Crit.Contracts.Enums;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Crit.Domain.Identity;
using Crit.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

// TODO: ***REFACTOR TO USE SERVICES***
[ApiController]
[Route("api")]
public class LoginController : ControllerBase
{
    private readonly Logging.IFileLogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserRepository _userRepository;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly IPhoneNumberRepository _phoneNumberRepository;
    private readonly ILoginService _loginService;
    public LoginController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        Logging.IFileLogger logger,
        IUserRepository userRepository,
        IOrganizationRepository organizationRepository,
        IEmailRepository emailRepository,
        IPhoneNumberRepository phoneNumberRepository,
        ILoginService loginService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _organizationRepository = organizationRepository;
        _emailRepository = emailRepository;
        _phoneNumberRepository = phoneNumberRepository;
        _loginService = loginService;
    }

    [HttpPost]
    [Route("Register")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> Register([FromBody] UserRequest user)
    {
        try
        {
            LoginResponse response = await _loginService.RegisterAsync(user);
            return GetLoginResponse(response);
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
    public async Task<IActionResult> Login([FromBody] UserRequest user, [FromQuery] bool? useCookies)
    {
        try
        {
            LoginResponse response = await _loginService.LoginAsync(user, useCookies);
            return GetLoginResponse(response);
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

    private IActionResult GetLoginResponse(LoginResponse response)
    {
        if (response.ErrorType.HasValue)
        {
            return response.ErrorType.Value switch
            {
                LoginErrorType.NullUser => BadRequest(response.Message),
                LoginErrorType.NullPassword => BadRequest(response.Message),
                LoginErrorType.UserExists => BadRequest(response.Message),
                LoginErrorType.MissingOrganizationName => BadRequest(response.Message),
                LoginErrorType.InvalidCredentials => Unauthorized(response.Message),
                LoginErrorType.Unauthorized => Unauthorized(response.Message),
                LoginErrorType.NullUserName => BadRequest(response.Message),
                _ => throw new NotImplementedException()
            };
        }
        else
        {
            return Ok(response);
        }
    }
}