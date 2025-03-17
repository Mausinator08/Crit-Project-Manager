using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace CritDTO.Identity;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<string>
{
}
