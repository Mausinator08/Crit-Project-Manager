using System.Text.Json.Serialization;
using Crit.Domain.Identity;

namespace Crit.Domain.Models;

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
