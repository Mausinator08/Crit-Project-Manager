using Crit.Contracts.Identity;
using Crit.Contracts.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetLoggedInUser();
    Task<Organization?> GetLoggedInUserOrganization();
}
