using System.Security.Claims;
using Crit.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Crit.Application.Users;

public class CurrentUserService : ICurrentUserService
{
	public bool IsAuthenticated { get; set; }

	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly UserManager<ApplicationUser> _userManager;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
	{
		IsAuthenticated = false;
		_httpContextAccessor = httpContextAccessor;
		_userManager = userManager;
	}

	public async Task<ApplicationUser?> GetLoggedInUserAsync()
	{
		try
		{
			ApplicationUser? loggedInUser = null;
			ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;

			if (user != null && user.Identity != null && user?.Identity?.IsAuthenticated == true)
			{
				loggedInUser = await _userManager.GetUserAsync(user);
			}

			return loggedInUser;
		}
		catch (Exception ex)
		{
			throw new Exception("Failed to get logged in user.", ex);
		}
	}
}
