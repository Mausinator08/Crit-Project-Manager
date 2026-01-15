using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;

namespace Crit.Application.Organizations;

public interface IOrganizationService
{
	Task<List<OrganizationResponse>> GetAllOrganizationsForLoggedInUser();
	Task<List<OrganizationResponse>> GetAllOrganizationsForUserId(Guid userId);
	Task<OrganizationResponse?> GetPrimaryOrganizationForLoggedInUser();
	Task<OrganizationResponse?> GetPrimaryOrganizationForUserId(Guid userId);
	Task<bool> AnyOrganizationsByName(string name);
	Task<OrganizationResponse?> GetOrganizationByName(string organizationName);
	Task<List<OrganizationResponse>> GetAllOrganizationsForProjectId(Guid projectId);
	Task<OrganizationResponse?> GetOwningOrganizationForProjectId(Guid projectId);
	Task<OrganizationResponse?> GetOrganization(Guid organizationId);
	Task<OrganizationResponse> CreateOrganization(CreateOrganizationRequest organization);
	Task<OrganizationResponse> UpdateOrganization(Guid id, UpdateOrganizationRequest organization);
	Task DeleteOrganization(Guid organizationId);
}
