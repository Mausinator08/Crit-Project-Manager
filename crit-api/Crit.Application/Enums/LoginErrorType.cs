namespace Crit.Application.Enums;

public enum LoginErrorType
{
	NullUser,
	NullUserName,
	NullPassword,
	UserExists,
	MissingOrganizationName,
	InvalidCredentials,
	Unauthorized
}
