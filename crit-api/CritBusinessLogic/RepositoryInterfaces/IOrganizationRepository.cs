using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IOrganizationRepository
{
    Task<Organization> CreateFirstOrganization(Organization organization);
    Task<List<Organization>> GetAllOrganizations();
    Task<List<Organization>> GetAllOrganizationsForProjectId(string projectId);
    Task<List<Organization>> GetAllOrganizationsForUserId(string userId);
    Task<Organization> GetOrganizationByUserId(string userId);
    Task<Organization> GetOrganization(string organizationId);
    Task<Organization> CreateOrganization(Organization organization);
    System.Threading.Tasks.Task UpdateOrganization(Organization organization);
    System.Threading.Tasks.Task DeleteOrganization(string organizationId);
}
