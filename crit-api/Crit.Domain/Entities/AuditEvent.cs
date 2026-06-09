namespace Crit.Domain.Entities;

public class AuditEvent : ICreatedAuditInfo, IUpdatedAuditInfo
{
    public Guid CreatedByUserId { get; set; } = Guid.Empty;
    public Guid UpdatedByUserId { get; set; } = Guid.Empty;
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}
