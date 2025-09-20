using System.Text.Json.Serialization;
using CritDTO.Identity;

namespace CritDTO.Models;

public class ProjectAdmin
{
	public ProjectAdmin()
	{
		ProjectId = Guid.Empty;
	}

	[JsonConstructor]
	public ProjectAdmin(Guid projectId, Guid adminId)
	{
		ProjectId = projectId;
		AdminId = adminId;
	}

	public Guid? Id { get; set; }
	public Guid ProjectId { get; set; }
	public Guid AdminId { get; set; }

	[JsonIgnore]
	public virtual Project? Project { get; set; }
	[JsonIgnore]
	public virtual ApplicationUser? Admin { get; set; }
}
