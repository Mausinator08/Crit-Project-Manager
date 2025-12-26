using Crit.Domain.Identity;
using Crit.Domain.Models;

namespace Crit.Application.RepositoryInterfaces;

// TODO: ***MOVE TO SERVICE! ALSO, MOVE CRUD OPS FROM API CONTROLLER TO HERE.***
public interface IUserRepository
{
    Task<ApplicationUser?> GetLoggedInUser();
    Task<Organization?> GetLoggedInUserOrganization();
}
