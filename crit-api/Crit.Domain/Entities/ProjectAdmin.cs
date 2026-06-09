
using Crit.Domain.Identity;
using Crit.Domain.Modules.ProjectManagement.Entities;

namespace Crit.Domain.Entities;

public class ProjectAdmin
{
	public ProjectAdmin()
	{
		ProjectId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid ProjectId { get; set; }
	public Guid AdminId { get; set; }

	public Project? Project { get; set; }
	public ApplicationUser? Admin { get; set; }
}
