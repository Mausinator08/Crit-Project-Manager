namespace Crit.Domain.Entities;

public interface IUpdatedAuditInfo
{
	Guid UpdatedByUserId { get; set; }
	DateTime DateUpdated { get; set; }
}
