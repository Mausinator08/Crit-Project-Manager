using System.Text;
using Crit.Application.RepositoryInterfaces;
using Crit.Contracts.Enums;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Crit.Domain.Enums;
using Crit.Domain.Identity;
using Crit.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Crit.Application.Logins;

public class LoginService : ILoginService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly IUserRepository _userRepository;
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly IOrganizationRepository _organizationRepository;
	private readonly IEmailRepository _emailRepository;
	private readonly IPhoneNumberRepository _phoneNumberRepository;
	public LoginService(
		UserManager<ApplicationUser> userManager,
		RoleManager<ApplicationRole> roleManager,
		SignInManager<ApplicationUser> signInManager,
		IUserRepository userRepository,
		IOrganizationRepository organizationRepository,
		IEmailRepository emailRepository,
		IPhoneNumberRepository phoneNumberRepository)
	{
		_userRepository = userRepository;
		_userManager = userManager;
		_roleManager = roleManager;
		_signInManager = signInManager;
		_organizationRepository = organizationRepository;
		_emailRepository = emailRepository;
		_phoneNumberRepository = phoneNumberRepository;
	}

	public async Task<LoginResponse> RegisterAsync(UserRequest user)
	{
		if (user == null)
		{
			return new LoginResponse()
			{
				Message = "Invalid user data.",
				ErrorType = LoginErrorType.NullUser
			};
		}

		if (user.Password == null)
		{
			return new LoginResponse()
			{
				Message = "Password is required.",
				ErrorType = LoginErrorType.NullPassword
			};
		}

		ApplicationUser appUser = new ApplicationUser() { UserName = user.UserName, Email = user.Email };

		if (appUser.UserName != null && await _userManager.FindByNameAsync(appUser.UserName) != null)
		{
			return new LoginResponse()
			{
				Message = "User already exists.",
				ErrorType = LoginErrorType.UserExists
			};
		}

		if (!_userManager.Users.Any() && !(await _userManager.GetUsersInRoleAsync("SuperAdmin")).Any())
		{
			IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

			if (!result.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create user.\n" + stringBuilder.ToString());
			}

			if (string.IsNullOrWhiteSpace(user.Organization))
			{
				return new LoginResponse()
				{
					Message = "Organization name is required.",
					ErrorType = LoginErrorType.MissingOrganizationName
				};
			}

			Organization organization = await _organizationRepository.CreateOrganization(new Organization() { Name = user.Organization, OwnerUserId = appUser.Id });

			if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
			{
				throw new Exception("Failed to create organization.");
			}

			Email email = await _emailRepository.CreateEmail(new Email()
			{
				EmailAddress = user.Email,
				OrganizationId = organization.Id.Value,
				Organization = organization,
				UserId = appUser.Id
			});

			PhoneNumber phoneNumber = await _phoneNumberRepository.CreatePhoneNumber(new PhoneNumber()
			{
				Number = user.PhoneNumber ?? string.Empty,
				OrganizationId = organization.Id.Value,
				Organization = organization,
				CountryCode = user.CountryCode ?? "+1",
				Extension = user.Extension,
				Type = user.PhoneType.HasValue ? user.PhoneType.Value : PhoneNumberType.Mobile,
				UserId = appUser.Id
			});

			if (string.IsNullOrWhiteSpace(user.Organization))
			{
				return new LoginResponse()
				{
					Message = "Organization name is required.",
					ErrorType = LoginErrorType.MissingOrganizationName
				};
			}

			IdentityResult createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "SuperAdmin" });

			if (!createRoleResult.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create SuperAdmin role.");
			}

			createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "OrganizationOwner" });

			if (!createRoleResult.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create OrganizationOwner role.");
			}

			createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "OrganizationAdmin" });

			if (!createRoleResult.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create OrganizationAdmin role.");
			}

			createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "ProjectOwner" });

			if (!createRoleResult.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create ProjectOwner role.");
			}

			createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "ProjectAdmin" });

			if (!createRoleResult.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create ProjectAdmin role.");
			}

			createRoleResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = "User" });

			if (!createRoleResult.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create User role.");
			}

			IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

			if (!roleResult.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create user with SuperAdmin role.");
			}

			roleResult = await _userManager.AddToRoleAsync(appUser, "OrganizationOwner");

			if (!roleResult.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create user with OrganizationOwner role.");
			}
		}
		else
		{
			IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

			if (!result.Succeeded)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IdentityError error in result.Errors)
				{
					stringBuilder.AppendLine(error.Description);
				}

				throw new Exception("Failed to create user.\n" + stringBuilder.ToString());
			}

			if (string.IsNullOrWhiteSpace(user.Organization))
			{
				return new LoginResponse()
				{
					Message = "Organization name is required.",
					ErrorType = LoginErrorType.MissingOrganizationName
				};
			}

			Organization? organization = null;
			List<Organization> organizationQuery = (await _organizationRepository.GetAllOrganizations()).Where(o => o.Name == user.Organization).ToList();

			if (!organizationQuery.Any())
			{
				organization = await _organizationRepository.CreateOrganization(new Organization() { Name = user.Organization, OwnerUserId = appUser.Id });

				IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "OrganizationOwner");

				if (!roleResult.Succeeded)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (IdentityError error in result.Errors)
					{
						stringBuilder.AppendLine(error.Description);
					}

					throw new Exception("Failed to create user with User role.");
				}
			}
			else
			{
				organization = organizationQuery.First();

				IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "User");

				if (!roleResult.Succeeded)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (IdentityError error in result.Errors)
					{
						stringBuilder.AppendLine(error.Description);
					}

					throw new Exception("Failed to create user with User role.");
				}
			}

			if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
			{
				throw new Exception("Failed to get or create organization.");
			}

			Email email = await _emailRepository.CreateEmail(new Email()
			{
				EmailAddress = user.Email,
				OrganizationId = organization.Id.Value,
				Organization = organization,
				UserId = appUser.Id
			});

			PhoneNumber phoneNumber = await _phoneNumberRepository.CreatePhoneNumber(new PhoneNumber()
			{
				Number = user.PhoneNumber ?? string.Empty,
				OrganizationId = organization.Id.Value,
				Organization = organization,
				CountryCode = user.CountryCode ?? "+1",
				Extension = user.Extension,
				Type = user.PhoneType.HasValue ? user.PhoneType.Value : PhoneNumberType.Mobile
			});
		}

		return new LoginResponse()
		{
			UserName = appUser.UserName,
			Message = $"Welcome to Crit! Enjoy your stay, {appUser.UserName}!"
		};
	}

	public async Task<LoginResponse> LoginAsync(UserRequest user, bool? useCookies)
	{
		if (user == null)
		{
			return new LoginResponse()
			{
				Message = "Invalid user data.",
				ErrorType = LoginErrorType.NullUser
			};
		}

		bool isPersistent = useCookies == true;

		// Simulate user authentication
		if (string.IsNullOrEmpty(user.UserName))
		{
			return new LoginResponse()
			{
				Message = "Username is required.",
				ErrorType = LoginErrorType.NullUserName
			};
		}

		ApplicationUser? applicationUser = await _userManager.FindByNameAsync(user.UserName);
		if (applicationUser != null)
		{
			Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(applicationUser, user.Password ?? string.Empty, isPersistent, false);

			if (result.RequiresTwoFactor)
			{
				if (!string.IsNullOrWhiteSpace(user.TwoFactorCode))
				{
					result = await _signInManager.TwoFactorAuthenticatorSignInAsync(user.TwoFactorCode, isPersistent, isPersistent);
				}
				else if (!string.IsNullOrWhiteSpace(user.TwoFactorRecoveryCode))
				{
					result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(user.TwoFactorRecoveryCode);
				}
			}

			if (!result.Succeeded)
			{
				return new LoginResponse()
				{
					Message = result.ToString(),
					ErrorType = LoginErrorType.Unauthorized
				};
			}

			return new LoginResponse()
			{
				UserName = applicationUser.UserName,
				Message = "You have logged in."
			};
		}
		else
		{
			return new LoginResponse()
			{
				Message = "Invalid credentials.",
				ErrorType = LoginErrorType.InvalidCredentials
			};
		}
	}
}
