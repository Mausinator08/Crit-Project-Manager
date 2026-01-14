using Crit.Domain.Identity;

namespace Crit.Application.Users;

public interface ICurrentUserService
{
	Task<ApplicationUser?> GetLoggedInUserAsync();
}
