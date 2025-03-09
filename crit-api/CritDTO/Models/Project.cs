using MongoDbGenericRepository.Attributes;

namespace CritDTO.Models;

public class Project : AuditInformation
{
    public Project()
    {
        Name = "";
        Tasks = new List<Task>();
        ProjectAdminUserIds = new List<Guid>();
        ProjectUserIds = new List<Guid>();
        OrganizationIds = new List<Guid>();
        CustomFieldTypes = new List<CustomFieldType>();
        HiddenCustomFieldTypes = new List<CustomFieldType>();
        AllowedStatusIds = new List<Guid>();
        Statuses = new List<Status>();
        AllowedStatuses = new List<Status>();
        Priorities = new List<Priority>();
        DateTime now = DateTime.Now;
        DateCreated = now;
        DateUpdated = now;
    }

    public Project(string name, string? description, Guid owningOrganizationId, Guid projectOwnerUserId)
    {
        Name = name;
        Description = description;
        Tasks = new List<Task>();
        OwningOrganizationId = owningOrganizationId;
        ProjectOwnerUserId = projectOwnerUserId;
        ProjectAdminUserIds = new List<Guid>([projectOwnerUserId]);
        ProjectUserIds = new List<Guid>([projectOwnerUserId]);
        OrganizationIds = new List<Guid>([owningOrganizationId]);
        CustomFieldTypes = new List<CustomFieldType>();
        HiddenCustomFieldTypes = new List<CustomFieldType>();
        Statuses = new List<Status>();
        AllowedStatusIds = new List<Guid>();
        AllowedStatuses = new List<Status>();
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
    public List<Guid> AllowedStatusIds { get; set; }

    public List<Status> Statuses { get; set; }
    public List<Status> AllowedStatuses { get; set; }
    public List<Priority> Priorities { get; set; }
    public List<CustomFieldType> CustomFieldTypes { get; set; }
    public List<CustomFieldType> HiddenCustomFieldTypes { get; set; }
    public List<Task> Tasks { get; set; }
}
