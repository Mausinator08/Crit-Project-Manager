using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Crit.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Crit.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
	public ApplicationUser() : base()
	{
		OwnedOrganizations = new List<Organization>();
		Projects = new List<Project>();
		MentionedUserComments = new List<MentionedUserComment>();
		OrganizationAdmins = new List<OrganizationAdmin>();
		OrganizationMembers = new List<OrganizationMember>();
		OrganizationAffiliates = new List<OrganizationAffiliate>();
		ProjectUsers = new List<ProjectUser>();
		ProjectAdmins = new List<ProjectAdmin>();
	}

	public ApplicationUser(string userName) : base(userName)
	{
		OwnedOrganizations = new List<Organization>();
		Projects = new List<Project>();
		MentionedUserComments = new List<MentionedUserComment>();
		OrganizationAdmins = new List<OrganizationAdmin>();
		OrganizationMembers = new List<OrganizationMember>();
		OrganizationAffiliates = new List<OrganizationAffiliate>();
		ProjectUsers = new List<ProjectUser>();
		ProjectAdmins = new List<ProjectAdmin>();
	}

	public List<Organization> OwnedOrganizations { get; set; }
	public List<Project> Projects { get; set; }
	public List<MentionedUserComment> MentionedUserComments { get; set; }
	public List<OrganizationAdmin> OrganizationAdmins { get; set; }
	public List<OrganizationMember> OrganizationMembers { get; set; }
	public List<OrganizationAffiliate> OrganizationAffiliates { get; set; }
	public List<ProjectUser> ProjectUsers { get; set; }
	public List<ProjectAdmin> ProjectAdmins { get; set; }
}
