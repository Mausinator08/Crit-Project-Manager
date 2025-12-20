using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Crit.Contracts.Identity;

[Table("Roles")]
public class ApplicationRole : IdentityRole<Guid>
{
}
