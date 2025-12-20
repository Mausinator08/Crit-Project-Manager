using Crit.Domain.Entities;
using Crit.Domain.Identity;

namespace Crit.Application.RepositoryInterfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetLoggedInUser();
    Task<Organization?> GetLoggedInUserOrganization();
}
