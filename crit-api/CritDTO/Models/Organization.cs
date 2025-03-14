using System.Text.Json.Serialization;
using MongoDbGenericRepository.Attributes;

namespace CritDTO.Models;

public class Organization
{
    public Organization()
    {
        Name = "";
        PhoneNumberIds = new List<Guid>();
        EmailIds = new List<Guid>();
        ProjectIds = new List<Guid>();
        AdminUserIds = new List<Guid>();
        MemberUserIds = new List<Guid>();
        AffiliatedUserIds = new List<Guid>();
        Emails = new List<Email>();
        PhoneNumbers = new List<PhoneNumber>();
    }

    [JsonConstructor]
    public Organization(string name, Guid ownerUserId)
    {
        Name = name;
        OwnerUserId = ownerUserId;
        PhoneNumberIds = new List<Guid>();
        EmailIds = new List<Guid>();
        ProjectIds = new List<Guid>();
        AdminUserIds = new List<Guid>([ownerUserId]);
        MemberUserIds = new List<Guid>([ownerUserId]);
        AffiliatedUserIds = new List<Guid>([ownerUserId]);
        Emails = new List<Email>();
        PhoneNumbers = new List<PhoneNumber>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerUserId { get; set; }
    public List<Guid> PhoneNumberIds { get; set; }
    public List<Guid> EmailIds { get; set; }
    public List<Guid> ProjectIds { get; set; }
    public List<Guid> AdminUserIds { get; set; }
    public List<Guid> MemberUserIds { get; set; }
    public List<Guid> AffiliatedUserIds { get; set; }
    public string DatabaseName { get => $"crit_{Id}"; }

    public List<Email> Emails { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; set; }
}
