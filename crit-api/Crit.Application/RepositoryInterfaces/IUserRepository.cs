using Crit.Domain.Identity;
using Crit.Domain.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetLoggedInUser();
    Task<Organization?> GetLoggedInUserOrganization();
}
