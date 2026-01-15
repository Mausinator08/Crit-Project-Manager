using Crit.Application.Mappers;
using Crit.Application.RepositoryInterfaces;
using Crit.Application.Users;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Crit.Domain.Identity;
using Crit.Domain.Models;

namespace Crit.Application.Organizations;

public class OrganizationService : IOrganizationService
{
	private readonly IOrganizationRepository _organizationRepository;
	private readonly IProjectsRepository _projectsRepository;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapperService _mapperService;

	public OrganizationService(IOrganizationRepository organizationRepository, IProjectsRepository projectsRepository, ICurrentUserService currentUserService, IMapperService mapperService)
	{
		_organizationRepository = organizationRepository;
		_projectsRepository = projectsRepository;
		_currentUserService = currentUserService;
		_mapperService = mapperService;
	}

	public async Task<List<OrganizationResponse>> GetAllOrganizationsForLoggedInUser()
	{
		ApplicationUser? loggedInAppUser = await _currentUserService.GetLoggedInUserAsync();

		if (loggedInAppUser != null)
		{
			List<Organization> organizations = await _organizationRepository.GetAllOrganizationsForUserId(loggedInAppUser.Id);

			if (organizations.Any())
			{
				return _mapperService.ConvertListTo<Organization, OrganizationResponse>(organizations);
			}
			else
			{
				throw new UnauthorizedAccessException("User is not a member of any organization.");
			}
		}

		return new List<OrganizationResponse>();
	}

	public async Task<List<OrganizationResponse>> GetAllOrganizationsForUserId(Guid userId)
	{
		List<Organization> organizations = await _organizationRepository.GetAllOrganizationsForUserId(userId);

		return _mapperService.ConvertListTo<Organization, OrganizationResponse>(organizations);
	}

	public async Task<bool> AnyOrganizationsByName(string name)
	{
		return await _organizationRepository.AnyOrganizationsByName(name);
	}

	public async Task<OrganizationResponse?> GetPrimaryOrganizationForLoggedInUser()
	{
		ApplicationUser? loggedInAppUser = await _currentUserService.GetLoggedInUserAsync();

		if (loggedInAppUser != null)
		{
			Organization? organizationAdmin = await _organizationRepository.GetOrganizationByAdminUserId(loggedInAppUser.Id);
			Organization? organizationMember = await _organizationRepository.GetOrganizationByMemberUserId(loggedInAppUser.Id);

			if (organizationAdmin != null)
			{
				// If user is an admin of an organization, then no need to continue to checking for member, for an admin is a member.
				return _mapperService.ConvertTo<Organization, OrganizationResponse>(organizationAdmin);
			}

			if (organizationMember != null)
			{
				return _mapperService.ConvertTo<Organization, OrganizationResponse>(organizationMember);
			}

			// It is invalid for a user to not be part of an organization of some sort. Even if the organization is simply the user themself.
			throw new InvalidOperationException($"{loggedInAppUser.UserName} is logged in but does not have a primary organization.");
		}

		return null;
	}

	public async Task<OrganizationResponse?> GetPrimaryOrganizationForUserId(Guid userId)
	{
		Organization? organizationAdmin = await _organizationRepository.GetOrganizationByAdminUserId(userId);
		Organization? organizationMember = await _organizationRepository.GetOrganizationByMemberUserId(userId);

		if (organizationAdmin != null)
		{
			// If user is an admin of an organization, then no need to continue to checking for member, for an admin is a member.
			return _mapperService.ConvertTo<Organization, OrganizationResponse>(organizationAdmin);
		}

		if (organizationMember != null)
		{
			return _mapperService.ConvertTo<Organization, OrganizationResponse>(organizationMember);
		}

		return null;
	}

	public async Task<OrganizationResponse?> GetOrganizationByName(string organizationName)
	{
		Organization? organization = (await _organizationRepository.GetAllOrganizations()).FirstOrDefault(o => o.Name == organizationName);

		if (organization == null)
		{
			return null;
		}

		return _mapperService.ConvertTo<Organization, OrganizationResponse>(organization);
	}

	public async Task<List<OrganizationResponse>> GetAllOrganizationsForProjectId(Guid projectId)
	{
		return _mapperService.ConvertListTo<Organization, OrganizationResponse>(await _organizationRepository.GetAllOrganizationsForProjectId(projectId));
	}

	public async Task<OrganizationResponse?> GetOwningOrganizationForProjectId(Guid projectId)
	{
		Project? project = await _projectsRepository.GetProject(projectId);

		if (project == null)
		{
			throw new Exception($"Project with Id {projectId} does not exist.");
		}

		Organization? owningOrganization = project.OwningOrganization;

		if (owningOrganization == null)
		{
			return null;
		}

		return _mapperService.ConvertTo<Organization, OrganizationResponse>(owningOrganization);
	}

	public async Task<OrganizationResponse?> GetOrganization(Guid organizationId)
	{
		Organization? organization = await _organizationRepository.GetOrganization(organizationId);

		return organization != null ? _mapperService.ConvertTo<Organization, OrganizationResponse>(organization) : null;
	}

	public async Task<OrganizationResponse> CreateOrganization(CreateOrganizationRequest organization)
	{
		if (string.IsNullOrWhiteSpace(organization.Name))
		{
			throw new ArgumentException("Organization name cannot be empty.", nameof(organization.Name));
		}

		List<Organization> existingOrganizations = await _organizationRepository.GetAllOrganizations();

		if (existingOrganizations.Any(o => o.Name == organization.Name))
		{
			throw new InvalidOperationException($"An organization with the name '{organization.Name}' already exists.");
		}

		if (existingOrganizations.Any(o =>
			o.OrganizationAdmins.Any(a => organization.OrganizationAdminIds.Any(ou => ou == a.AdminUserId)) ||
			o.OrganizationMembers.Any(m => organization.OrganizationMemberIds.Any(ou => ou == m.MemberUserId))))
		{
			throw new InvalidOperationException($"Admin and/or member user for organization {organization.Name} is already in another organization. Consider adding the user as an affiliate.");
		}

		Organization createdOrganization = await _organizationRepository.CreateOrganization(_mapperService.ConvertTo<CreateOrganizationRequest, Organization>(organization));

		return _mapperService.ConvertTo<Organization, OrganizationResponse>(createdOrganization);
	}

	public async Task<OrganizationResponse> UpdateOrganization(Guid id, UpdateOrganizationRequest organization)
	{
		List<Organization> existingOrganizations = await _organizationRepository.GetAllOrganizations();

		if (existingOrganizations.Any(o =>
			o.OrganizationAdmins.Any(a => organization.OrganizationAdminIds != null ? organization.OrganizationAdminIds.Any(ou => ou == a.AdminUserId) : false) ||
			o.OrganizationMembers.Any(m => organization.OrganizationMemberIds != null ? organization.OrganizationMemberIds.Any(ou => ou == m.MemberUserId) : false)))
		{
			throw new InvalidOperationException($"Admin and/or member user for organization {organization.Name} is already in another oganization. Consider adding the user as an affiliate.");
		}

		Organization? organizationToUpdate = await _organizationRepository.GetOrganization(id);

		if (organizationToUpdate == null)
		{
			throw new NullReferenceException($"Organization with ID {id} does not exist.");
		}

		organizationToUpdate.Name = organization.Name ?? organizationToUpdate.Name;
		organizationToUpdate.OwnerUserId = organization.OwnerUserId ?? organizationToUpdate.OwnerUserId;

		if (organization.OrganizationAdminIds != null)
		{
			foreach (Guid userId in organization.OrganizationAdminIds)
			{
				if (!organizationToUpdate.OrganizationAdmins.Any(ou => ou.AdminUserId == userId))
				{
					organizationToUpdate.OrganizationAdmins.Add(new OrganizationAdmin()
					{
						AdminUserId = userId,
						OrganizationId = id
					});
				}
			}

			List<OrganizationAdmin> organizationAdminsToDelete = organizationToUpdate.OrganizationAdmins.Where(ou => !organization.OrganizationAdminIds.Contains(ou.AdminUserId)).ToList();

			if (organizationAdminsToDelete.Any())
			{
				foreach (OrganizationAdmin user in organizationAdminsToDelete)
				{
					organizationToUpdate.OrganizationAdmins.Remove(user);
				}
			}
		}

		if (organization.OrganizationMemberIds != null)
		{
			foreach (Guid userId in organization.OrganizationMemberIds)
			{
				if (!organizationToUpdate.OrganizationMembers.Any(ou => ou.MemberUserId == userId))
				{
					organizationToUpdate.OrganizationMembers.Add(new OrganizationMember()
					{
						MemberUserId = userId,
						OrganizationId = id
					});
				}
			}

			List<OrganizationMember> organizationMembersToDelete = organizationToUpdate.OrganizationMembers.Where(ou => !organization.OrganizationMemberIds.Contains(ou.MemberUserId)).ToList();

			if (organizationMembersToDelete.Any())
			{
				foreach (OrganizationMember user in organizationMembersToDelete)
				{
					organizationToUpdate.OrganizationMembers.Remove(user);
				}
			}
		}

		if (organization.OrganizationAffiliateIds != null)
		{
			foreach (Guid userId in organization.OrganizationAffiliateIds)
			{
				if (!organizationToUpdate.OrganizationAffiliates.Any(ou => ou.AffiliateUserId == userId))
				{
					organizationToUpdate.OrganizationAffiliates.Add(new OrganizationAffiliate()
					{
						AffiliateUserId = userId,
						OrganizationId = id
					});
				}
			}

			List<OrganizationAffiliate> organizationAffiliatesToDelete = organizationToUpdate.OrganizationAffiliates.Where(ou => !organization.OrganizationAffiliateIds.Contains(ou.AffiliateUserId)).ToList();

			if (organizationAffiliatesToDelete.Any())
			{
				foreach (OrganizationAffiliate user in organizationAffiliatesToDelete)
				{
					organizationToUpdate.OrganizationAffiliates.Remove(user);
				}
			}
		}

		if (organization.ProjectIds != null)
		{
			foreach (Guid projectId in organization.ProjectIds)
			{
				if (!organizationToUpdate.Projects.Any(p => p.Id == projectId))
				{
					Project? project = await _projectsRepository.GetProject(projectId);

					if (project == null)
					{
						throw new NullReferenceException($"Project with id {projectId} does not exist.");
					}

					organizationToUpdate.Projects.Add(project);
				}
			}

			List<Project> projectsToDelete = organizationToUpdate.Projects.Where(p => p.Id != null ? !organization.ProjectIds.Contains(p.Id.Value) : false).ToList();

			if (projectsToDelete.Any())
			{
				foreach (Project project in projectsToDelete)
				{
					organizationToUpdate.Projects.Remove(project);
				}
			}
		}

		Organization updatedOrganization = await _organizationRepository.UpdateOrganization(organizationToUpdate);
		return _mapperService.ConvertTo<Organization, OrganizationResponse>(updatedOrganization);
	}

	public async Task DeleteOrganization(Guid organizationId)
	{
		List<Project> projects = await _projectsRepository.GetAllProjects();

		projects = projects.Where(p => p.OwningOrganizationId == organizationId).ToList();

		if (projects.Any())
		{
			throw new InvalidOperationException($"Cannot delete organization {organizationId} because it owns projects.");
		}

		await _organizationRepository.DeleteOrganization(organizationId);
	}
}
