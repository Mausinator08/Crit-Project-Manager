using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class OrganizationAdminResponse
{
	public OrganizationAdminResponse()
	{
		OrganizationId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid AdminUserId { get; set; }
}
