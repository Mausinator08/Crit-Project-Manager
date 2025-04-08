using CritApi.Models;
using CritBusinessLogic.RepositoryInterfaces;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize()]
public class UserController : ControllerBase
{
    private readonly Logging.ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IPhoneNumberRepository _phoneNumberRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public UserController(UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    Logging.ILogger logger,
    IPhoneNumberRepository phoneNumberRepository,
    IOrganizationRepository organizationRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _phoneNumberRepository = phoneNumberRepository;
        _organizationRepository = organizationRepository;
    }

    [HttpPost]
    [Authorize(Roles = "ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> CreateUser(User user, string role = "User")
    {
        try
        {
            if (role == "SuperAdmin")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ApiResult("Could not create user.", new List<string>([$"The {role} role cannot be created."]), user));
            }

            ApplicationUser appUser = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            if (await _roleManager.FindByNameAsync(role) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResult("Could not create user.", new List<string>([$"{role} role does not exist."]), user));
            }

            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password ?? string.Empty);
            IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, role);

            if (result.Succeeded && roleResult.Succeeded)
            {
                return Ok(new ApiResult("User created successfully.", null, user));
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (IdentityError error in result.Errors)
                {
                    errors.Add(error.Code + " - " + error.Description);
                }

                foreach (IdentityError error in roleResult.Errors)
                {
                    errors.Add(error.Code + " - " + error.Description);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult("Could not create user.", errors, user));
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to create user.");
        }
    }

    [HttpPut]
    [Authorize(Roles = "ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> UpdateUser(User user)
    {
        try
        {
            if (user.UserName == null || user.Email == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResult("Could not update user.", new List<string>(["User name and email are required."]), user));
            }

            ApplicationUser? appUser = await _userManager.FindByNameAsync(user.UserName);

            if (appUser != null)
            {
                if ((await _userManager.GetUsersInRoleAsync("SuperAdmin")).Where(u => u.UserName == appUser.UserName).Any())
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiResult("Could not update user.", new List<string>([$"The {appUser.UserName} SuperAdmin user cannot be updated."]), user));
                }

                appUser.Email = user.Email;
                IdentityResult result = await _userManager.UpdateAsync(appUser);

                user.Id = appUser.Id;

                if (result.Succeeded)
                {
                    return Ok(new ApiResult("User updated successfully.", null, user));
                }
                else
                {
                    List<string> errors = new List<string>();
                    foreach (IdentityError error in result.Errors)
                    {
                        errors.Add(error.Code + " - " + error.Description);
                    }

                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult("Could not update user.", errors, user));
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResult("Could not update user.", new List<string>([$"User {user.UserName} not found."]), user));
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to update user.");
        }
    }

    [HttpDelete]
    [Authorize(Roles = "ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> DeleteUser(string userName)
    {
        try
        {
            ApplicationUser? appUser = _userManager.Users.Where(predicate: au => string.Equals(au.UserName, userName)).FirstOrDefault();

            if (appUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResult("Could not delete user.", new List<string>([$"The user {userName} could not be found."])));
            }

            if (appUser.UserName == null && appUser.Email == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult("Could not delete user.", new List<string>([$"The user {userName} has no stored user name or email."])));
            }

            if ((await _userManager.GetUsersInRoleAsync("SuperAdmin")).Where(u => u.UserName == appUser.UserName).Any())
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ApiResult("Could not delete user.", new List<string>([$"The {appUser.UserName} SuperAdmin user cannot be deleted."]), new User(appUser.UserName, appUser.Email)));
            }

            IdentityResult result = await _userManager.DeleteAsync(appUser);

            if (result.Succeeded && appUser.UserName != null && appUser.Email != null)
            {
                return Ok(new ApiResult("User deleted successfully.", null, new User(appUser.UserName, appUser.Email)));
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (IdentityError error in result.Errors)
                {
                    errors.Add(error.Code + " - " + error.Description);
                }

                if (appUser.UserName != null && appUser.Email != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult("Could not delete user.", errors, new User(appUser.UserName, appUser.Email)));
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult("Could not delete user.", errors));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to delete user.");
        }
    }

    [HttpGet]
    [Route("{userId}")]
    [Authorize(Roles = "User,ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> GetUserByUserId([FromRoute] string userId)
    {
        try
        {
            ApplicationUser? appUser = await _userManager.FindByIdAsync(userId);

            if (appUser != null && appUser?.Email != null && appUser?.UserName != null)
            {
                if ((await _userManager.GetUsersInRoleAsync("SuperAdmin")).Where(u => u.UserName == appUser.UserName).Any())
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiResult("Could not get user.", new List<string>([$"The {appUser.UserName} SuperAdmin user cannot be retrieved."]), new User(appUser.UserName, appUser.Email)));
                }

                PhoneNumber phoneNumber = await _phoneNumberRepository.GetPhoneNumberByUserId(userId);
                Organization organization = await _organizationRepository.GetOrganization(userId);

                User user = new User(appUser.UserName, appUser.Email);

                user.Id = appUser.Id;
                user.CountryCode = phoneNumber.CountryCode;
                user.PhoneNumber = phoneNumber.Number;
                user.PhoneType = phoneNumber.Type;
                user.Extension = phoneNumber.Extension;
                user.Organization = organization.Name;

                return Ok(new ApiResult("User retrieved successfully.", null, user));
            }
            else
            {
                List<string> errors = new List<string>();

                if (appUser == null)
                {
                    errors.Add($"User with user id {userId} does not exist.");
                }

                if (appUser?.Email == null)
                {
                    errors.Add($"User with user id {userId} was found, but this user has no email.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult("Could not get user.", errors));
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to get user.");
        }
    }


    [HttpGet]
    [Route("GetUserByUserName/{userName}")]
    [Authorize(Roles = "User,ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> GetUserByUserName([FromRoute] string userName)
    {
        try
        {
            ApplicationUser? appUser = await _userManager.FindByNameAsync(userName);

            if (appUser != null && appUser?.Email != null && appUser?.UserName != null)
            {
                if ((await _userManager.GetUsersInRoleAsync("SuperAdmin")).Where(u => u.UserName == appUser.UserName).Any())
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiResult("Could not get user.", new List<string>([$"The {appUser.UserName} SuperAdmin user cannot be retrieved."]), new User(appUser.UserName, appUser.Email)));
                }

                PhoneNumber phoneNumber = await _phoneNumberRepository.GetPhoneNumberByUserId(appUser.Id);
                Organization organization = await _organizationRepository.GetOrganization(appUser.Id);

                User user = new User(appUser.UserName, appUser.Email);

                user.Id = appUser.Id;
                user.CountryCode = phoneNumber.CountryCode;
                user.PhoneNumber = phoneNumber.Number;
                user.PhoneType = phoneNumber.Type;
                user.Extension = phoneNumber.Extension;
                user.Organization = organization.Name;

                return Ok(new ApiResult("User retrieved successfully.", null, user));
            }
            else
            {
                List<string> errors = new List<string>();

                if (appUser == null)
                {
                    errors.Add($"User with user name {userName} does not exist.");
                }

                if (appUser?.Email == null)
                {
                    errors.Add($"User with user name {userName} was found, but this user has no email.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult("Could not get user.", errors));
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to get user.");
        }
    }

    [HttpGet]
    [Route("GetUserByEmail/{email}")]
    [Authorize(Roles = "User,ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
    {
        try
        {
            ApplicationUser? appUser = await _userManager.FindByEmailAsync(email);

            if (appUser != null && appUser?.Email != null && appUser?.UserName != null)
            {
                if ((await _userManager.GetUsersInRoleAsync("SuperAdmin")).Where(u => u.UserName == appUser.UserName).Any())
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiResult("Could not get user.", new List<string>([$"The {appUser.UserName} SuperAdmin user cannot be retrieved."]), new User(appUser.UserName, appUser.Email)));
                }

                PhoneNumber phoneNumber = await _phoneNumberRepository.GetPhoneNumberByUserId(appUser.Id);
                Organization organization = await _organizationRepository.GetOrganization(appUser.Id);

                User user = new User(appUser.UserName, appUser.Email);

                user.Id = appUser.Id;
                user.CountryCode = phoneNumber.CountryCode;
                user.PhoneNumber = phoneNumber.Number;
                user.PhoneType = phoneNumber.Type;
                user.Extension = phoneNumber.Extension;
                user.Organization = organization.Name;

                return Ok(new ApiResult("User retrieved successfully.", null, user));
            }
            else
            {
                List<string> errors = new List<string>();

                if (appUser == null)
                {
                    errors.Add($"User with email {email} does not exist.");
                }

                if (appUser?.UserName == null)
                {
                    errors.Add($"User with email {email} was found, but this user has no user name.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult("Could not get user.", errors));
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to get user.");
        }
    }

    [HttpGet]
    [Authorize(Roles = "User,ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            List<ApplicationUser> superAdminUsers = (await _userManager.GetUsersInRoleAsync("SuperAdmin")).ToList();
            List<ApplicationUser> appUsers = await _userManager.Users.Where(u => !superAdminUsers.Contains(u)).ToListAsync();

            if (appUsers.Any())
            {
                List<User> users = new List<User>();

                int countOfUsersWithNoData = 0;
                foreach (ApplicationUser appUser in appUsers)
                {
                    if (appUser.UserName != null && appUser.Email != null)
                    {
                        PhoneNumber phoneNumber = await _phoneNumberRepository.GetPhoneNumberByUserId(appUser.Id);
                        Organization organization = await _organizationRepository.GetOrganization(appUser.Id);

                        User user = new User(appUser.UserName, appUser.Email);

                        user.Id = appUser.Id;
                        user.CountryCode = phoneNumber.CountryCode;
                        user.PhoneNumber = phoneNumber.Number;
                        user.PhoneType = phoneNumber.Type;
                        user.Extension = phoneNumber.Extension;
                        user.Organization = organization.Name;

                        users.Add(user);
                    }
                    else
                    {
                        countOfUsersWithNoData++;
                    }
                }

                if (countOfUsersWithNoData == 0)
                {
                    return Ok(new ApiResult("All users retrieved successfully.", null, users));
                }
                else
                {
                    return StatusCode(StatusCodes.Status206PartialContent, new ApiResult("Only some users were retrieved successfully.", new List<string>([$"{countOfUsersWithNoData} users had missing data."]), users));
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult("Could not get all users.", new List<string>(["No users exist."])));
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to get all users.");
        }
    }

    [HttpGet]
    [Route("GetAllUsersInRole/{role}")]
    [Authorize(Roles = "User,ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> GetAllUsersInRole([FromRoute] string role)
    {
        try
        {
            if (role == "SuperAdmin")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ApiResult("Could not get all users for role.", new List<string>([$"The {role} role cannot be retrieved."])));
            }

            IList<ApplicationUser> appUsers = await _userManager.GetUsersInRoleAsync(role);

            if (appUsers.Any())
            {
                List<User> users = new List<User>();

                int countOfUsersWithNoData = 0;
                foreach (ApplicationUser appUser in appUsers)
                {
                    if (appUser.UserName != null && appUser.Email != null)
                    {
                        PhoneNumber phoneNumber = await _phoneNumberRepository.GetPhoneNumberByUserId(appUser.Id);
                        Organization organization = await _organizationRepository.GetOrganization(appUser.Id);

                        User user = new User(appUser.UserName, appUser.Email);

                        user.Id = appUser.Id;
                        user.CountryCode = phoneNumber.CountryCode;
                        user.PhoneNumber = phoneNumber.Number;
                        user.PhoneType = phoneNumber.Type;
                        user.Extension = phoneNumber.Extension;
                        user.Organization = organization.Name;

                        users.Add(user);
                    }
                    else
                    {
                        countOfUsersWithNoData++;
                    }
                }

                if (countOfUsersWithNoData == 0)
                {
                    return Ok(new ApiResult($"All users retrieved successfully for {role} role.", null, users));
                }
                else
                {
                    return StatusCode(StatusCodes.Status206PartialContent, new ApiResult($"Only some users were retrieved successfully for {role} role.", new List<string>([$"{countOfUsersWithNoData} users had missing data."]), users));
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult($"Could not get all users for {role} role.", new List<string>([$"No users exist within the {role} role."])));
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Server Error: Failed to get all users for {role} role.");
        }
    }
}
