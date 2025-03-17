namespace CritDTO.Models;

public class AuditInformation
{
    public string CreatedByUserId { get; set; } = string.Empty;
    public string UpdatedByUserId { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}
