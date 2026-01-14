using Crit.Domain.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IOrganizationRepository
{
    Task<bool> AnyOrganizationsByName(string name);
    Task<List<Organization>> GetAllOrganizations();
    Task<List<Organization>> GetAllOrganizationsForProjectId(Guid projectId);
    Task<List<Organization>> GetAllOrganizationsForUserId(Guid userId);
    Task<Organization?> GetOrganizationByAdminUserId(Guid userId);
    Task<Organization?> GetOrganizationByMemberUserId(Guid userId);
    Task<Organization?> GetOrganization(Guid organizationId);
    Task<Organization?> CreateOrganization(Organization organization);
    Task UpdateOrganization(Organization organization);
    Task DeleteOrganization(Guid organizationId);
}
