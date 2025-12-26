using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class ApplicationUserResponse
{
	public ApplicationUserResponse()
	{
		OwnedOrganizations = new List<OrganizationResponse>();
		Projects = new List<ProjectResponse>();
		MentionedUserComments = new List<MentionedUserCommentResponse>();
		OrganizationAdmins = new List<OrganizationAdminResponse>();
		OrganizationMembers = new List<OrganizationMemberResponse>();
		OrganizationAffiliates = new List<OrganizationAffiliateResponse>();
		ProjectUsers = new List<ProjectUserResponse>();
		ProjectAdmins = new List<ProjectAdminResponse>();
	}

	public List<OrganizationResponse> OwnedOrganizations { get; set; }
	public List<ProjectResponse> Projects { get; set; }
	public List<MentionedUserCommentResponse> MentionedUserComments { get; set; }
	public List<OrganizationAdminResponse> OrganizationAdmins { get; set; }
	public List<OrganizationMemberResponse> OrganizationMembers { get; set; }
	public List<OrganizationAffiliateResponse> OrganizationAffiliates { get; set; }
	public List<ProjectUserResponse> ProjectUsers { get; set; }
	public List<ProjectAdminResponse> ProjectAdmins { get; set; }
}
