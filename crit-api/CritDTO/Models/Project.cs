using System.Text.Json.Serialization;
using MongoDbGenericRepository.Attributes;

namespace CritDTO.Models;

public class Project : AuditInformation
{
    public Project()
    {
        Name = "";
        Tasks = new List<ProjectTask>();
        ProjectAdminUserIds = new List<Guid>();
        ProjectUserIds = new List<Guid>();
        OrganizationIds = new List<Guid>();
        CustomFieldTypes = new List<CustomFieldType>();
        HiddenCustomFieldTypeIds = new List<Guid>();
        Statuses = new List<Status>();
        Priorities = new List<Priority>();
        DateTime now = DateTime.Now;
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
        ProjectAdminUserIds = new List<Guid>([projectOwnerUserId]);
        ProjectUserIds = new List<Guid>([projectOwnerUserId]);
        OrganizationIds = new List<Guid>([owningOrganizationId]);
        CustomFieldTypes = new List<CustomFieldType>();
        HiddenCustomFieldTypeIds = new List<Guid>();
        Statuses = new List<Status>();
        Priorities = new List<Priority>();
        DateTime now = DateTime.Now;
        DateCreated = now;
        DateUpdated = now;
        CreatedByUserId = projectOwnerUserId;
        UpdatedByUserId = projectOwnerUserId;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid OwningOrganizationId { get; set; }
    public Guid ProjectOwnerUserId { get; set; }
    public List<Guid> ProjectAdminUserIds { get; set; }
    public List<Guid> ProjectUserIds { get; set; }
    public List<Guid> OrganizationIds { get; set; }
    public List<Guid> HiddenCustomFieldTypeIds { get; set; }

    public List<Status> Statuses { get; set; }
    public List<Priority> Priorities { get; set; }
    public List<CustomFieldType> CustomFieldTypes { get; set; }
    public List<ProjectTask> Tasks { get; set; }
}
