using MongoDbGenericRepository.Attributes;

namespace CritDTO.Models;

public class Organization
{
    public Organization()
    {
        Name = "";
        PhoneNumbers = new List<PhoneNumber>();
        Emails = new List<Email>();
        ProjectIds = new List<Guid>();
        AdminUserIds = new List<Guid>();
        MemberUserIds = new List<Guid>();
        affiliatedUserIds = new List<Guid>();
    }

    public Organization(string name, Guid ownerUserId)
    {
        Name = name;
        OwnerUserId = ownerUserId;
        PhoneNumbers = new List<PhoneNumber>();
        Emails = new List<Email>();
        ProjectIds = new List<Guid>();
        AdminUserIds = new List<Guid>([ownerUserId]);
        MemberUserIds = new List<Guid>([ownerUserId]);
        affiliatedUserIds = new List<Guid>([ownerUserId]);
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerUserId { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; set; }
    public List<Email> Emails { get; set; }
    public List<Guid> ProjectIds { get; set; }
    public List<Guid> AdminUserIds { get; set; }
    public List<Guid> MemberUserIds { get; set; }
    public List<Guid> affiliatedUserIds { get; set; }
    public string DatabaseName { get => $"crit_{Id}"; }
}
