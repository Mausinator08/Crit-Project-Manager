namespace Crit.Application.Logins;

public interface ILoginService
{
	async Task<IActionResult> Register(User user)
}
