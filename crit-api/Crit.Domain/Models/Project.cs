using System.Text.Json.Serialization;
using Crit.Domain.BaseModels;
using Crit.Domain.Identity;

namespace Crit.Domain.Entities;

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

    [JsonConstructor]
    public Project(string name, string? description, Guid owningOrganizationId, Guid projectOwnerUserId)
    {
        Name = name;
        Description = description;
        Tasks = new List<ProjectTask>();
        OwningOrganizationId = owningOrganizationId;
        ProjectOwnerUserId = projectOwnerUserId;
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
        CreatedByUserId = projectOwnerUserId;
        UpdatedByUserId = projectOwnerUserId;
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid OwningOrganizationId { get; set; }
    public Guid ProjectOwnerUserId { get; set; }

    [JsonIgnore]
    public virtual Organization? OwningOrganization { get; set; }
    [JsonIgnore]
    public virtual ApplicationUser? ProjectOwner { get; set; }
    [JsonIgnore]
    public virtual List<Status> Statuses { get; set; }
    [JsonIgnore]
    public virtual List<Priority> Priorities { get; set; }
    [JsonIgnore]
    public virtual List<CustomFieldType> CustomFieldTypes { get; set; }
    [JsonIgnore]
    public virtual List<ProjectTask> Tasks { get; set; }
    [JsonIgnore]
    public virtual List<Organization> Organizations { get; set; }
    [JsonIgnore]
    public virtual List<CustomFieldType> HiddenCustomFieldTypes { get; set; }
    [JsonIgnore]
    public virtual List<Comment> Comments { get; set; }
    [JsonIgnore]
    public virtual List<OrganizationProject> OrganizationProjects { get; set; }
    [JsonIgnore]
    public virtual List<ProjectUser> ProjectUsers { get; set; }
    [JsonIgnore]
    public virtual List<ProjectAdmin> ProjectAdmins { get; set; }
}
