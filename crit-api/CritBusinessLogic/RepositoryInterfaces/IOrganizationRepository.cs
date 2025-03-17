using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IOrganizationRepository
{
    Task<Organization> CreateFirstOrganization(Organization organization);
    Task<List<Organization>> GetAllOrganizations();
    Task<Organization?> GetOrganization(string organizationId);
    Task<Organization> CreateOrganization(Organization organization);
    System.Threading.Tasks.Task UpdateOrganization(Organization organization);
    System.Threading.Tasks.Task DeleteOrganization(string organizationId);
}
