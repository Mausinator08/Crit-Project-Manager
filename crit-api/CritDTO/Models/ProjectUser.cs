using System.Text.Json.Serialization;
using CritDTO.Identity;

namespace CritDTO.Models;

public class ProjectUser
{
	public ProjectUser()
	{
		ProjectId = Guid.Empty;
	}

	[JsonConstructor]
	public ProjectUser(Guid projectId, Guid userId)
	{
		ProjectId = projectId;
		UserId = userId;
	}

	public Guid? Id { get; set; }
	public Guid ProjectId { get; set; }
	public Guid UserId { get; set; }

	[JsonIgnore]
	public virtual Project? Project { get; set; }
	[JsonIgnore]
	public virtual ApplicationUser? User { get; set; }
}
