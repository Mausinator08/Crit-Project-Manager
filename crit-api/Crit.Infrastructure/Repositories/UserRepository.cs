using System.Security.Authentication;
using System.Security.Claims;
using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Contexts;
using Crit.Domain.Identity;
using Crit.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Crit.Infrastructure.Repositories;

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

            List<Organization> organizationsMemberQuery = await _critDbContext.Organizations.AsNoTracking().Where(o => o.OrganizationMembers.Any(ou => ou.MemberUserId == applicationUser.Id)).ToListAsync();
            List<Organization> organizationsAdminQuery = await _critDbContext.Organizations.AsNoTracking().Where(o => o.OrganizationAdmins.Any(ou => ou.AdminUserId == applicationUser.Id)).ToListAsync();

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
