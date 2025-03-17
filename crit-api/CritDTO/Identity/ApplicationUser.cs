using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace CritDTO.Identity;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<string>
{
}
