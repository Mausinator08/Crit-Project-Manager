using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Crit.Domain.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
}
