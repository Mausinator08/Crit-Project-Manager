using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;

namespace Crit.Application.Logins;

public interface ILoginService
{
	Task<LoginResponse> RegisterAsync(CreateUserRequest user);
	Task<LoginResponse> LoginAsync(CreateUserRequest user, bool? useCookies);
}
