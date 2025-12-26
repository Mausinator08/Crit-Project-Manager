using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class OrganizationResponse
{
    public OrganizationResponse()
    {
        Name = "";
        Emails = new List<EmailResponse>();
        PhoneNumbers = new List<PhoneNumberResponse>();
        OwnerUserId = Guid.Empty;
        Projects = new List<ProjectResponse>();
        OrganizationAdmins = new List<OrganizationAdminResponse>();
        OrganizationMembers = new List<OrganizationMemberResponse>();
        OrganizationAffiliates = new List<OrganizationAffiliateResponse>();
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerUserId { get; set; }

    public List<EmailResponse> Emails { get; set; }
    public List<PhoneNumberResponse> PhoneNumbers { get; set; }
    public List<ProjectResponse> Projects { get; set; }
    public List<OrganizationAdminResponse> OrganizationAdmins { get; set; }
    public List<OrganizationMemberResponse> OrganizationMembers { get; set; }
    public List<OrganizationAffiliateResponse> OrganizationAffiliates { get; set; }
}
