using System.Text.Json.Serialization;
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

    [JsonIgnore]
    public List<Email> Emails { get; set; }
    [JsonIgnore]
    public List<PhoneNumber> PhoneNumbers { get; set; }
    [JsonIgnore]
    public List<Project> Projects { get; set; }
    [JsonIgnore]
    public ApplicationUser? Owner { get; set; }
    [JsonIgnore]
    public List<OrganizationProject> OrganizationProjects { get; set; }
    [JsonIgnore]
    public List<OrganizationAdmin> OrganizationAdmins { get; set; }
    [JsonIgnore]
    public List<OrganizationMember> OrganizationMembers { get; set; }
    [JsonIgnore]
    public List<OrganizationAffiliate> OrganizationAffiliates { get; set; }
}
