using Crit.Contracts.Enums;

namespace Crit.Contracts.ResponseModels;

public class LoginResponse
{
	public string? UserName { get; set; }
	public string? Message { get; set; }
	public LoginErrorType? ErrorType { get; set; }
}
