using System.Text.Json.Serialization;
using Crit.Domain.Identity;

namespace Crit.Domain.Entities;

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

    [JsonConstructor]
    public Organization(string name, Guid ownerUserId)
    {
        Name = name;
        OwnerUserId = ownerUserId;
        Emails = new List<Email>();
        PhoneNumbers = new List<PhoneNumber>();
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
    public virtual List<Email> Emails { get; set; }
    [JsonIgnore]
    public virtual List<PhoneNumber> PhoneNumbers { get; set; }
    [JsonIgnore]
    public virtual List<Project> Projects { get; set; }
    [JsonIgnore]
    public virtual ApplicationUser? Owner { get; set; }
    [JsonIgnore]
    public virtual List<OrganizationProject> OrganizationProjects { get; set; }
    [JsonIgnore]
    public virtual List<OrganizationAdmin> OrganizationAdmins { get; set; }
    [JsonIgnore]
    public virtual List<OrganizationMember> OrganizationMembers { get; set; }
    [JsonIgnore]
    public virtual List<OrganizationAffiliate> OrganizationAffiliates { get; set; }
}
