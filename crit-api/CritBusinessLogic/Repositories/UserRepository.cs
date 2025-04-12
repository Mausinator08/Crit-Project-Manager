using System.Security.Authentication;
using System.Security.Claims;
using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CritDbContext _critDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserRepository(UserManager<ApplicationUser> userManager, CritDbContext critDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _critDbContext = critDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApplicationUser?> GetLoggedInUser()
    {
        try
        {
            ClaimsPrincipal? user = null;

            if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                user = _httpContextAccessor.HttpContext.User;
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            ApplicationUser? applicationUser = await _userManager.GetUserAsync(user);

            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser));
            }

            return applicationUser;
        }
        catch (Exception ex)
        {
            throw new Exception("Could not get logged in user.", ex);
        }
    }

    public async Task<Organization?> GetLoggedInUserOrganization()
    {
        try
        {
            ClaimsPrincipal? user = null;

            if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                user = _httpContextAccessor.HttpContext.User;
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            ApplicationUser? applicationUser = await _userManager.GetUserAsync(user);

            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser));
            }

            IQueryable<Organization> organizationsMemberQuery = _critDbContext.Organizations.AsNoTracking().Where(o => o.MemberUserIds.Contains(applicationUser.Id));
            IQueryable<Organization> organizationsAdminQuery = _critDbContext.Organizations.AsNoTracking().Where(o => o.AdminUserIds.Contains(applicationUser.Id));

            Organization? organization = null;

            if (organizationsMemberQuery.Any())
            {
                organization = organizationsMemberQuery.First();
            }
            else if (organizationsAdminQuery.Any())
            {
                organization = organizationsAdminQuery.First();
            }
            else
            {
                throw new AuthenticationException("User is not a member of any organization.");
            }

            return organization;
        }
        catch (Exception ex)
        {
            throw new Exception("Could not get logged in user organization.", ex);
        }
    }

}
