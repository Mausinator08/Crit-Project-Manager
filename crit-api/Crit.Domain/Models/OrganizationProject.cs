using System.Text.Json.Serialization;

namespace Crit.Domain.Entities;

public class OrganizationProject
{
	public OrganizationProject()
	{
		OrganizationId = Guid.Empty;
		ProjectId = Guid.Empty;
	}

	[JsonConstructor]
	public OrganizationProject(Guid organizationId, Guid projectId)
	{
		OrganizationId = organizationId;
		ProjectId = projectId;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid ProjectId { get; set; }

	[JsonIgnore]
	public virtual Organization? Organization { get; set; }
	[JsonIgnore]
	public virtual Project? Project { get; set; }
}
