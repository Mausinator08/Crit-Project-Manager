using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class ProjectAdminResponse
{
	public ProjectAdminResponse()
	{
		ProjectId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid ProjectId { get; set; }
	public Guid AdminId { get; set; }
}
