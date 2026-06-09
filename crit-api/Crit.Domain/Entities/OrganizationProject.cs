using Crit.Domain.Modules.ProjectManagement.Entities;

namespace Crit.Domain.Modules.OrganizationManagement.Entities;

public class OrganizationProject
{
	public OrganizationProject()
	{
		OrganizationId = Guid.Empty;
		ProjectId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid ProjectId { get; set; }

	public Organization? Organization { get; set; }
	public Project? Project { get; set; }
}
