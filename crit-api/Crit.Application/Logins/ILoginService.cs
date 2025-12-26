using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;

namespace Crit.Application.Logins;

public interface ILoginService
{
	Task<LoginResponse> RegisterAsync(UserRequest user);
	Task<LoginResponse> LoginAsync(UserRequest user, bool? useCookies);
}
