using CritDTO.Identity;
using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetLoggedInUser();
    Task<Organization?> GetLoggedInUserOrganization();
}
