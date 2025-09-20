using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CritBusinessLogic.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly CritDbContext _critDbContext;
    private readonly IUserRepository _userRepository;
    private readonly IProjectsRepository _projectsRepository;

    public OrganizationRepository(CritDbContext critDbContext, IUserRepository userRepository, IProjectsRepository projectsRepository)
    {
        _critDbContext = critDbContext;
        _userRepository = userRepository;
        _projectsRepository = projectsRepository;
    }

    public async Task<Organization> CreateFirstOrganization(Organization organization)
    {
        try
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization), "Organization cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(organization.Name))
            {
                throw new ArgumentException("Organization name cannot be empty.", nameof(organization.Name));
            }

            if (await _critDbContext.Organizations.AnyAsync(o => o.Name == organization.Name))
            {
                throw new InvalidOperationException($"An organization with the name '{organization.Name}' already exists.");
            }

            if (organization.OwnerUserId == Guid.Empty)
            {
                throw new Exception("All organizations must have an owner.");
            }

            EntityEntry<Organization> savedOrganization = _critDbContext.Organizations.Add(organization);

            if (savedOrganization == null || savedOrganization.Entity == null)
            {
                throw new Exception($"The organization {organization.Name} did not save correctly.");
            }

            if (savedOrganization.Entity.Id == null || savedOrganization.Entity.Id == Guid.Empty)
            {
                throw new Exception($"The project {organization.Name} did not generate the Id correctly.");
            }

            _critDbContext.OrganizationAdmins.Add(new OrganizationAdmin(savedOrganization.Entity.Id.Value, organization.OwnerUserId));
            _critDbContext.OrganizationMembers.Add(new OrganizationMember(savedOrganization.Entity.Id.Value, organization.OwnerUserId));
            _critDbContext.OrganizationAffiliates.Add(new OrganizationAffiliate(savedOrganization.Entity.Id.Value, organization.OwnerUserId));

            int savedChanges = await _critDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Failed to save organization.");
            }

            return organization;
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException || ex is ArgumentNullException || ex is ArgumentException || ex is UnauthorizedAccessException)
            {
                throw;
            }

            throw new Exception("Error creating the first organization.", ex);
        }
    }

    public async Task<List<Organization>> GetAllOrganizations()
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new NullReferenceException("CritDbContext is null.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();

            if (organization != null && applicationUser != null)
            {
                List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o =>
                o.Id == organization.Id &&
                o.OrganizationAffiliates.Any(ou => ou.AffiliateUserId == applicationUser.Id)).ToListAsync();

                if (organizationsQuery.Any())
                {
                    return organizationsQuery;
                }
            }
            else if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new UnauthorizedAccessException("User is not a member of any organization.");
            }

            return new List<Organization>();
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedAccessException || ex is NullReferenceException)
            {
                throw;
            }

            throw new Exception("Error retrieving all organizations.", ex);
        }
    }

    public async Task<List<Organization>> GetAllOrganizationsForProjectId(Guid projectId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new NullReferenceException("CritDbContext is null.");
            }

            List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o => o.Projects.Any(p => p.Id == projectId)).ToListAsync();

            if (!organizationsQuery.Any())
            {
                throw new InvalidOperationException($"No organizations found for project with ID {projectId}.");
            }

            return organizationsQuery;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving organizations for project with ID {projectId}.", ex);
        }
    }

    public async Task<List<Organization>> GetAllOrganizationsForUserId(Guid userId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new NullReferenceException("CritDbContext is null.");
            }

            List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o =>
            o.OrganizationAdmins.Any(ou => ou.AdminUserId == userId) ||
            o.OrganizationMembers.Any(ou => ou.MemberUserId == userId) ||
            o.OrganizationAffiliates.Any(ou => ou.AffiliateUserId == userId)).ToListAsync();

            if (!organizationsQuery.Any())
            {
                throw new InvalidOperationException($"No organizations found for user with ID {userId}.");
            }

            return organizationsQuery;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving organizations for user with ID {userId}.", ex);
        }
    }

    public async Task<Organization> GetOrganizationByUserId(Guid userId)
    {
        if (_critDbContext == null)
        {
            throw new NullReferenceException("CritDbContext is null.");
        }

        List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o =>
        o.OrganizationAdmins.Any(ou => ou.AdminUserId == userId) ||
        o.OrganizationMembers.Any(ou => ou.MemberUserId == userId)).ToListAsync();

        if (!organizationsQuery.Any())
        {
            throw new InvalidOperationException($"User with ID {userId} is not a member of any organization.");
        }

        return organizationsQuery.First();
    }

    public async Task<Organization> GetOrganization(Guid organizationId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new NullReferenceException("CritDbContext is null.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();

            if (applicationUser != null)
            {
                List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o =>
                o.Id == organizationId &&
                o.OrganizationAffiliates.Any(ou => ou.AffiliateUserId == applicationUser.Id)).ToListAsync();

                if (organizationsQuery.Any())
                {
                    return organizationsQuery.First();
                }

                throw new InvalidOperationException($"User with ID {applicationUser.Id} is not a member or affilate of organization with ID {organizationId}.");
            }
            else
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedAccessException || ex is NullReferenceException)
            {
                throw;
            }

            throw new Exception($"Error retrieving organization with ID {organizationId}.", ex);
        }
    }

    public async Task<Organization> CreateOrganization(Organization organization)
    {
        try
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization), "Organization cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(organization.Name))
            {
                throw new ArgumentException("Organization name cannot be empty.", nameof(organization.Name));
            }

            if (await _critDbContext.Organizations.AnyAsync(o => o.Name == organization.Name))
            {
                throw new InvalidOperationException($"An organization with the name '{organization.Name}' already exists.");
            }

            List<Organization> organizationsQuery = _critDbContext.Organizations.AsNoTracking().AsEnumerable().Where(o =>
            o.OrganizationAdmins.Where(a => organization.OrganizationAdmins.Any(ou => ou.AdminUserId == a.AdminUserId)).Any() ||
            o.OrganizationMembers.Where(m => organization.OrganizationMembers.Any(ou => ou.MemberUserId == m.MemberUserId)).Any()).ToList();

            if (organizationsQuery.Any())
            {
                throw new InvalidOperationException($"Admin and/or member user for organization {organization.Id} is already in another oganization. Consider adding the user as an affiliate.");
            }

            EntityEntry<Organization> savedOrganization = _critDbContext.Organizations.Add(organization);

            if (savedOrganization == null || savedOrganization.Entity == null)
            {
                throw new Exception($"The organization {organization.Name} did not save correctly.");
            }

            if (savedOrganization.Entity.Id == null || savedOrganization.Entity.Id == Guid.Empty)
            {
                throw new Exception($"The project {organization.Name} did not generate the Id correctly.");
            }

            _critDbContext.OrganizationAdmins.Add(new OrganizationAdmin(savedOrganization.Entity.Id.Value, organization.OwnerUserId));
            _critDbContext.OrganizationMembers.Add(new OrganizationMember(savedOrganization.Entity.Id.Value, organization.OwnerUserId));
            _critDbContext.OrganizationAffiliates.Add(new OrganizationAffiliate(savedOrganization.Entity.Id.Value, organization.OwnerUserId));

            int savedChanges = await _critDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Failed to save organization.");
            }

            return organization;
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException || ex is ArgumentNullException || ex is ArgumentException)
            {
                throw;
            }

            throw new Exception("Error creating the organization.", ex);
        }
    }

    public async Task UpdateOrganization(Organization organization)
    {
        try
        {
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(organization), "Organization cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(organization.Name))
            {
                throw new ArgumentException("Organization name cannot be empty.", nameof(organization.Name));
            }

            List<Organization> organizationsQuery = _critDbContext.Organizations.AsNoTracking().AsEnumerable().Where(o =>
            o.Id == organization.Id &&
            o.OrganizationAdmins.Where(a => organization.OrganizationAdmins.Any(ou => ou.AdminUserId == a.AdminUserId)).Any() ||
            o.OrganizationMembers.Where(m => organization.OrganizationMembers.Any(ou => ou.MemberUserId == m.MemberUserId)).Any()).ToList();

            if (organizationsQuery.Any())
            {
                throw new InvalidOperationException($"Admin and/or member user for organization {organization.Id} is already in another oganization. Consider adding the user as an affiliate.");
            }

            _critDbContext.Organizations.Update(organization);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException || ex is ArgumentNullException || ex is ArgumentException)
            {
                throw;
            }

            throw new Exception("Error updating the organization.", ex);
        }
    }

    public async Task DeleteOrganization(Guid organizationId)
    {
        try
        {
            if (_critDbContext == null)
            {
                throw new NullReferenceException("CritDbContext is null.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();

            if (applicationUser == null)
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o => o.Id == organizationId && o.OrganizationAdmins.Any(ou => ou.AdminUserId == applicationUser.Id)).ToListAsync();

            if (!organizationsQuery.Any())
            {
                throw new InvalidOperationException($"User with ID {applicationUser.Id} is not an administrator of organization with ID {organizationId}.");
            }

            Organization organization = organizationsQuery.First();

            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new InvalidOperationException($"Organization with ID {organizationId} does not exist.");
            }

            if (_critDbContext == null)
            {
                throw new NullReferenceException("CritDbContext is null.");
            }

            List<Project> projectsQuery = await _critDbContext.Projects.AsNoTracking().Where(p => p.OwningOrganizationId == organizationId).ToListAsync();

            if (projectsQuery.Any())
            {
                throw new InvalidOperationException($"Cannot delete organization {organizationId} because it owns projects.");
            }

            _critDbContext.Organizations.Remove(organization);
            await _critDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException || ex is UnauthorizedAccessException || ex is NullReferenceException)
            {
                throw;
            }

            throw new Exception($"Error deleting organization with ID {organizationId}.", ex);
        }
    }
}
