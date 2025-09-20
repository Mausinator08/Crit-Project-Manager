using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CritDTO.Identity;

[Table("Roles")]
public class ApplicationRole : IdentityRole<Guid>
{
}
