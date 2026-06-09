
using Crit.Domain.Identity;
using Crit.Domain.Modules.ProjectManagement.Entities;

namespace Crit.Domain.Entities;

public class ProjectUser
{
	public ProjectUser()
	{
		ProjectId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid ProjectId { get; set; }
	public Guid UserId { get; set; }

	public Project? Project { get; set; }
	public ApplicationUser? User { get; set; }
}
