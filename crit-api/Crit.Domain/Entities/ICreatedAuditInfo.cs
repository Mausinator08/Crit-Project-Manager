namespace Crit.Domain.Entities;

public interface ICreatedAuditInfo
{
	Guid CreatedByUserId { get; set; }
	DateTime DateCreated { get; set; }
}
