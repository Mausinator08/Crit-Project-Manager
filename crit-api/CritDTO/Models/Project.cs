using System.Text.Json.Serialization;
using MongoDbGenericRepository.Attributes;

namespace CritDTO.Models;

public class Project : AuditInformation
{
    public Project()
    {
        Name = "";
        Tasks = new List<ProjectTask>();
        OwningOrganizationId = string.Empty;
        ProjectOwnerUserId = string.Empty;
        ProjectAdminUserIds = new List<string>();
        ProjectUserIds = new List<string>();
        OrganizationIds = new List<string>();
        CustomFieldTypes = new List<CustomFieldType>();
        HiddenCustomFieldTypeIds = new List<string>();
        Statuses = new List<Status>();
        Priorities = new List<Priority>();
        DateTime now = DateTime.Now;
        DateCreated = now;
        DateUpdated = now;
    }

    [JsonConstructor]
    public Project(string name, string? description, string owningOrganizationId, string projectOwnerUserId)
    {
        Name = name;
        Description = description;
        Tasks = new List<ProjectTask>();
        OwningOrganizationId = owningOrganizationId;
        ProjectOwnerUserId = projectOwnerUserId;
        ProjectAdminUserIds = new List<string>([projectOwnerUserId]);
        ProjectUserIds = new List<string>([projectOwnerUserId]);
        OrganizationIds = new List<string>([owningOrganizationId]);
        CustomFieldTypes = new List<CustomFieldType>();
        HiddenCustomFieldTypeIds = new List<string>();
        Statuses = new List<Status>();
        Priorities = new List<Priority>();
        DateTime now = DateTime.Now;
        DateCreated = now;
        DateUpdated = now;
        CreatedByUserId = projectOwnerUserId;
        UpdatedByUserId = projectOwnerUserId;
    }

    public string? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string OwningOrganizationId { get; set; }
    public string ProjectOwnerUserId { get; set; }
    public List<string> ProjectAdminUserIds { get; set; }
    public List<string> ProjectUserIds { get; set; }
    public List<string> OrganizationIds { get; set; }
    public List<string> HiddenCustomFieldTypeIds { get; set; }

    public List<Status> Statuses { get; set; }
    public List<Priority> Priorities { get; set; }
    public List<CustomFieldType> CustomFieldTypes { get; set; }
    public List<ProjectTask> Tasks { get; set; }
}
