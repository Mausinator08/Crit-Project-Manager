using System.Text.Json.Serialization;
using MongoDbGenericRepository.Attributes;

namespace CritDTO.Models;

public class Organization
{
    public Organization()
    {
        Name = "";
        PhoneNumberIds = new List<string>();
        EmailIds = new List<string>();
        ProjectIds = new List<string>();
        AdminUserIds = new List<string>();
        MemberUserIds = new List<string>();
        AffiliatedUserIds = new List<string>();
        Emails = new List<Email>();
        PhoneNumbers = new List<PhoneNumber>();
        OwnerUserId = string.Empty;
    }

    [JsonConstructor]
    public Organization(string name, string ownerUserId)
    {
        Name = name;
        OwnerUserId = ownerUserId;
        PhoneNumberIds = new List<string>();
        EmailIds = new List<string>();
        ProjectIds = new List<string>();
        AdminUserIds = new List<string>([ownerUserId]);
        MemberUserIds = new List<string>([ownerUserId]);
        AffiliatedUserIds = new List<string>([ownerUserId]);
        Emails = new List<Email>();
        PhoneNumbers = new List<PhoneNumber>();
    }

    public string? Id { get; set; }
    public string Name { get; set; }
    public string OwnerUserId { get; set; }
    public List<string> PhoneNumberIds { get; set; }
    public List<string> EmailIds { get; set; }
    public List<string> ProjectIds { get; set; }
    public List<string> AdminUserIds { get; set; }
    public List<string> MemberUserIds { get; set; }
    public List<string> AffiliatedUserIds { get; set; }
    public string DatabaseName { get => $"crit_{Id}"; }

    public List<Email> Emails { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; set; }
}
