using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Crit.Domain.Identity;

[Table("Roles")]
public class ApplicationRole : IdentityRole<Guid>
{
}
