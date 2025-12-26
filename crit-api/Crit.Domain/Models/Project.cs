using System.Text.Json.Serialization;
using Crit.Domain.BaseModels;
using Crit.Domain.Identity;

namespace Crit.Domain.Models;

public class Project : AuditInformation
{
    public Project()
    {
        Name = "";
        Tasks = new List<ProjectTask>();
        OwningOrganizationId = Guid.Empty;
        ProjectOwnerUserId = Guid.Empty;
        Organizations = new List<Organization>();
        CustomFieldTypes = new List<CustomFieldType>();
        Statuses = new List<Status>();
        Priorities = new List<Priority>();
        HiddenCustomFieldTypes = new List<CustomFieldType>();
        Comments = new List<Comment>();
        OrganizationProjects = new List<OrganizationProject>();
        ProjectUsers = new List<ProjectUser>();
        ProjectAdmins = new List<ProjectAdmin>();
        DateTime now = DateTime.UtcNow;
        DateCreated = now;
        DateUpdated = now;
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid OwningOrganizationId { get; set; }
    public Guid ProjectOwnerUserId { get; set; }

    public Organization? OwningOrganization { get; set; }
    public ApplicationUser? ProjectOwner { get; set; }
    public List<Status> Statuses { get; set; }
    public List<Priority> Priorities { get; set; }
    public List<CustomFieldType> CustomFieldTypes { get; set; }
    public List<ProjectTask> Tasks { get; set; }
    public List<Organization> Organizations { get; set; }
    public List<CustomFieldType> HiddenCustomFieldTypes { get; set; }
    public List<Comment> Comments { get; set; }
    public List<OrganizationProject> OrganizationProjects { get; set; }
    public List<ProjectUser> ProjectUsers { get; set; }
    public List<ProjectAdmin> ProjectAdmins { get; set; }
}
