
using Crit.Domain.Identity;
namespace Crit.Domain.Models;

public class Organization
{
    public Organization()
    {
        Name = "";
        Emails = new List<Email>();
        PhoneNumbers = new List<PhoneNumber>();
        OwnerUserId = Guid.Empty;
        Projects = new List<Project>();
        OrganizationProjects = new List<OrganizationProject>();
        OrganizationAdmins = new List<OrganizationAdmin>();
        OrganizationMembers = new List<OrganizationMember>();
        OrganizationAffiliates = new List<OrganizationAffiliate>();
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerUserId { get; set; }

    public List<Email> Emails { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; set; }
    public List<Project> Projects { get; set; }
    public ApplicationUser? Owner { get; set; }
    public List<OrganizationProject> OrganizationProjects { get; set; }
    public List<OrganizationAdmin> OrganizationAdmins { get; set; }
    public List<OrganizationMember> OrganizationMembers { get; set; }
    public List<OrganizationAffiliate> OrganizationAffiliates { get; set; }
}
