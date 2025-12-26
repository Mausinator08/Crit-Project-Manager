using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class ProjectUserResponse
{
	public ProjectUserResponse()
	{
		ProjectId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid ProjectId { get; set; }
	public Guid UserId { get; set; }
}
