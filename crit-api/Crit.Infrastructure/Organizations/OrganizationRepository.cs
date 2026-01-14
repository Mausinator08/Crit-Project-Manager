using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Models;
using Crit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Crit.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly CritDbContext _critDbContext;

    public OrganizationRepository(CritDbContext critDbContext, IProjectsRepository projectsRepository)
    {
        _critDbContext = critDbContext;
    }

    public async Task<bool> AnyOrganizationsByName(string name)
    {
        return await _critDbContext.Organizations.AnyAsync(o => o.Name == name);
    }

    public async Task<List<Organization>> GetAllOrganizations()
    {
        if (_critDbContext == null)
        {
            throw new NullReferenceException("CritDbContext is null.");
        }

        List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().ToListAsync();

        return organizationsQuery;
    }

    public async Task<List<Organization>> GetAllOrganizationsForProjectId(Guid projectId)
    {
        if (_critDbContext == null)
        {
            throw new NullReferenceException("CritDbContext is null.");
        }

        List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o => o.Projects.Any(p => p.Id == projectId)).ToListAsync();

        return organizationsQuery;
    }

    public async Task<List<Organization>> GetAllOrganizationsForUserId(Guid userId)
    {
        if (_critDbContext == null)
        {
            throw new NullReferenceException("CritDbContext is null.");
        }

        List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o =>
        o.OrganizationAdmins.Any(ou => ou.AdminUserId == userId) ||
        o.OrganizationMembers.Any(ou => ou.MemberUserId == userId) ||
        o.OrganizationAffiliates.Any(ou => ou.AffiliateUserId == userId)).ToListAsync();

        return organizationsQuery;
    }

    public async Task<Organization?> GetOrganizationByAdminUserId(Guid userId)
    {
        if (_critDbContext == null)
        {
            throw new NullReferenceException("CritDbContext is null.");
        }

        List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o =>
        o.OrganizationAdmins.Any(ou => ou.AdminUserId == userId)).ToListAsync();

        if (!organizationsQuery.Any())
        {
            return null;
        }

        return organizationsQuery.First();
    }

    public async Task<Organization?> GetOrganizationByMemberUserId(Guid userId)
    {
        if (_critDbContext == null)
        {
            throw new NullReferenceException("CritDbContext is null.");
        }

        List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o =>
        o.OrganizationMembers.Any(ou => ou.MemberUserId == userId)).ToListAsync();

        if (!organizationsQuery.Any())
        {
            return null;
        }

        return organizationsQuery.First();
    }

    public async Task<Organization?> GetOrganization(Guid organizationId)
    {
        if (_critDbContext == null)
        {
            throw new NullReferenceException("CritDbContext is null.");
        }

        List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o =>
        o.Id == organizationId).ToListAsync();

        if (!organizationsQuery.Any())
        {
            return null;
        }

        return organizationsQuery.First();
    }

    public async Task<Organization?> CreateOrganization(Organization organization)
    {
        if (organization == null)
        {
            throw new ArgumentNullException(nameof(organization), "Organization cannot be null.");
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

        _critDbContext.OrganizationAdmins.Add(new OrganizationAdmin() { OrganizationId = savedOrganization.Entity.Id.Value, AdminUserId = organization.OwnerUserId });
        _critDbContext.OrganizationMembers.Add(new OrganizationMember() { OrganizationId = savedOrganization.Entity.Id.Value, MemberUserId = organization.OwnerUserId });
        _critDbContext.OrganizationAffiliates.Add(new OrganizationAffiliate() { OrganizationId = savedOrganization.Entity.Id.Value, AffiliateUserId = organization.OwnerUserId });

        int savedChanges = await _critDbContext.SaveChangesAsync();

        if (savedChanges <= 0)
        {
            throw new Exception("Failed to save organization.");
        }

        return savedOrganization.Entity;
    }

    public async Task UpdateOrganization(Organization organization)
    {
        if (organization.Id == null || organization.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(organization), "Organization must have a valid id.");
        }

        _critDbContext.Organizations.Update(organization);
        await _critDbContext.SaveChangesAsync();
    }

    public async Task DeleteOrganization(Guid organizationId)
    {
        if (_critDbContext == null)
        {
            throw new NullReferenceException("CritDbContext is null.");
        }

        List<Organization> organizationsQuery = await _critDbContext.Organizations.AsNoTracking().Where(o => o.Id == organizationId).ToListAsync();

        if (!organizationsQuery.Any())
        {
            throw new InvalidOperationException($"Organization with ID {organizationId} does not exist.");
        }

        Organization organization = organizationsQuery.First();

        _critDbContext.Organizations.Remove(organization);
        await _critDbContext.SaveChangesAsync();
    }
}
