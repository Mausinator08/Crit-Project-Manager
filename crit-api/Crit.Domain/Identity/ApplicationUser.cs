using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Crit.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Crit.Domain.Identity;

[Table("Users")]
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

	[JsonIgnore]
	public virtual List<Organization> OwnedOrganizations { get; set; }
	[JsonIgnore]
	public virtual List<Project> Projects { get; set; }
	[JsonIgnore]
	public virtual List<MentionedUserComment> MentionedUserComments { get; set; }
	[JsonIgnore]
	public virtual List<OrganizationAdmin> OrganizationAdmins { get; set; }
	[JsonIgnore]
	public virtual List<OrganizationMember> OrganizationMembers { get; set; }
	[JsonIgnore]
	public virtual List<OrganizationAffiliate> OrganizationAffiliates { get; set; }
	[JsonIgnore]
	public virtual List<ProjectUser> ProjectUsers { get; set; }
	[JsonIgnore]
	public virtual List<ProjectAdmin> ProjectAdmins { get; set; }
}
