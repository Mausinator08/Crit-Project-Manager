using System.Text.Json.Serialization;
using Crit.Domain.Identity;

namespace Crit.Domain.Models;

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
