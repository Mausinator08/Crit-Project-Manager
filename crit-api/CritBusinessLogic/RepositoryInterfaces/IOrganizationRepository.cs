using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IOrganizationRepository
{
    Task<Organization> CreateFirstOrganization(Organization organization);
    Task<List<Organization>> GetAllOrganizations();
    Task<List<Organization>> GetAllOrganizationsForProjectId(Guid projectId);
    Task<List<Organization>> GetAllOrganizationsForUserId(Guid userId);
    Task<Organization> GetOrganizationByUserId(Guid userId);
    Task<Organization> GetOrganization(Guid organizationId);
    Task<Organization> CreateOrganization(Organization organization);
    Task UpdateOrganization(Organization organization);
    Task DeleteOrganization(Guid organizationId);
}
