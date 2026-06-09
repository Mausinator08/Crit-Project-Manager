using Crit.Domain.Entities;
using Crit.Domain.Modules.OrganizationManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crit.Application.Contracts.Persistence;

public interface ICritContext
{
	DbSet<AuditEvent> AuditEvents { get; }
	DbSet<Email> Emails { get; }
	DbSet<PhoneNumber> PhoneNumbers { get; }
	DbSet<OrganizationAdmin> OrganizationAdmins { get; }
	DbSet<OrganizationAffiliate> OrganizationAffiliates { get; }
	DbSet<OrganizationMember> OrganizationMembers { get; }
	DbSet<OrganizationProject> OrganizationProjects { get; }
	DbSet<ProjectAdmin> ProjectAdmins { get; }
	DbSet<ProjectUser> ProjectUsers { get; }
}
