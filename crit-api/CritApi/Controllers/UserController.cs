using CritApi.Models;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize()]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost]
    [Route("CreateUser")]
    [Authorize(Roles = "ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner")]
    public async Task<IActionResult> CreateUser(User user, string role = "User")
    {
        try
        {
            ApplicationUser appUser = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            if (await _roleManager.FindByNameAsync(role) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResult("Could not create user.", new List<string>([$"{role} role does not exist."]), user));
            }

            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to create user.");
        }
    }

    [HttpPost]
    [Route("UpdateUser")]
    [Authorize(Roles = "ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner")]
    public async Task<IActionResult> UpdateUser(User user)
    {
        try
        {
            ApplicationUser? appUser = await _userManager.FindByNameAsync(user.UserName);

            if (appUser != null)
            {
                appUser.Email = user.Email;

                IdentityResult result = await _userManager.UpdateAsync(appUser);

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
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to update user.");
        }
    }

    [HttpPost]
    [Route("DeleteUser")]
    [Authorize(Roles = "ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner")]
    public async Task<IActionResult> DeleteUser(string userName)
    {
        try
        {
            ApplicationUser? appUser = _userManager.Users.Where(predicate: au => string.Equals(au.UserName, userName)).FirstOrDefault();

            if (appUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResult("Could not delete user.", new List<string>([$"The user {userName} could not be found."])));
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to delete user.");
        }
    }

    [HttpGet]
    [Route("GetUserByUserName")]
    [Authorize(Roles = "User;ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner")]
    public async Task<IActionResult> GetUserByUserName(string userName)
    {
        try
        {
            ApplicationUser? appUser = await _userManager.FindByNameAsync(userName);

            if (appUser != null && appUser?.Email != null && appUser?.UserName != null)
            {
                return Ok(new ApiResult("User retrieved successfully.", null, new User(appUser.UserName, appUser.Email)));
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to get user.");
        }
    }

    [HttpGet]
    [Route("GetUserByEmail")]
    [Authorize(Roles = "User;ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        try
        {
            ApplicationUser? appUser = await _userManager.FindByEmailAsync(email);

            if (appUser != null && appUser?.Email != null && appUser?.UserName != null)
            {
                return Ok(new ApiResult("User retrieved successfully.", null, new User(appUser.UserName, appUser.Email)));
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to get user.");
        }
    }

    [HttpGet]
    [Route("GetAllUsers")]
    [Authorize(Roles = "User;ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            List<ApplicationUser> appUsers = _userManager.Users.ToList();

            if (appUsers.Any())
            {
                List<User> users = new List<User>();

                int countOfUsersWithNoData = 0;
                foreach (ApplicationUser appUser in appUsers)
                {
                    if (appUser.UserName != null && appUser.Email != null)
                    {
                        users.Add(new User(appUser.UserName, appUser.Email));
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Server Error: Failed to get all users.");
        }
    }

    [HttpGet]
    [Route("GetAllUsersInRole")]
    [Authorize(Roles = "User;ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner")]
    public async Task<IActionResult> GetAllUsersInRole(string role)
    {
        try
        {
            IList<ApplicationUser> appUsers = await _userManager.GetUsersInRoleAsync(role);

            if (appUsers.Any())
            {
                List<User> users = new List<User>();

                int countOfUsersWithNoData = 0;
                foreach (ApplicationUser appUser in appUsers)
                {
                    if (appUser.UserName != null && appUser.Email != null)
                    {
                        users.Add(new User(appUser.UserName, appUser.Email));
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
            return StatusCode(StatusCodes.Status500InternalServerError, $"Server Error: Failed to get all users for {role} role.");
        }
    }
}
