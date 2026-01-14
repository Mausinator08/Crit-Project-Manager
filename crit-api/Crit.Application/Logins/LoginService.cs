using System.Text;
using Crit.Application.Emails;
using Crit.Application.Organizations;
using Crit.Contracts.Emails;
using Crit.Contracts.Enums;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Crit.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Crit.Application.Logins;

public class LoginService : ILoginService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly IOrganizationService _organizationService;
	private readonly IEmailService _emailService;
	private readonly IPhoneNumberService _phoneNumberService;
	public LoginService(
		UserManager<ApplicationUser> userManager,
		RoleManager<ApplicationRole> roleManager,
		SignInManager<ApplicationUser> signInManager,
		IOrganizationService organizationService,
		IEmailService emailService,
		IPhoneNumberService phoneNumberService)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_signInManager = signInManager;
		_organizationService = organizationService;
		_emailService = emailService;
		_phoneNumberService = phoneNumberService;
	}

	public async Task<LoginResponse> RegisterAsync(CreateUserRequest user)
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

			OrganizationResponse? organization = await _organizationService.CreateOrganization(new CreateOrganizationRequest()
			{
				Name = user.Organization,
				OwnerUserId = appUser.Id,
				CountryCode = user.CountryCode,
				Email = user.Email,
				Extension = user.Extension,
				NumberType = user.PhoneType,
				PhoneNumber = user.PhoneNumber,
				OrganizationAdminIds = new List<Guid>([appUser.Id]),
				OrganizationMemberIds = new List<Guid>([appUser.Id]),
				OrganizationAffiliateIds = new List<Guid>([appUser.Id])
			});

			if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
			{
				throw new Exception("Failed to create organization.");
			}

			EmailResponse? email = await _emailService.CreateEmail(new CreateEmailRequest()
			{
				EmailAddress = user.Email,
				OrganizationId = organization.Id.Value,
				UserId = appUser.Id
			});

			PhoneNumberResponse? phoneNumber = await _phoneNumberService.CreatePhoneNumber(new CreatePhoneNumberRequest()
			{
				Number = user.PhoneNumber ?? string.Empty,
				OrganizationId = organization.Id.Value,
				CountryCode = user.CountryCode ?? "+1",
				Extension = user.Extension,
				Type = user.PhoneType,
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

			OrganizationResponse? organization = await _organizationService.GetOrganizationByName(user.Organization);

			if (organization == null)
			{
				organization = await _organizationService.CreateOrganization(new CreateOrganizationRequest()
				{
					Name = user.Organization,
					OwnerUserId = appUser.Id,
					CountryCode = user.CountryCode,
					Email = user.Email,
					Extension = user.Extension,
					NumberType = user.PhoneType,
					PhoneNumber = user.PhoneNumber,
					OrganizationAdminIds = new List<Guid>([appUser.Id]),
					OrganizationMemberIds = new List<Guid>([appUser.Id]),
					OrganizationAffiliateIds = new List<Guid>([appUser.Id])
				});

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

			EmailResponse? email = await _emailService.CreateEmail(new CreateEmailRequest()
			{
				EmailAddress = user.Email,
				OrganizationId = organization.Id.Value,
				UserId = appUser.Id
			});

			PhoneNumberResponse? phoneNumber = await _phoneNumberService.CreatePhoneNumber(new CreatePhoneNumberRequest()
			{
				Number = user.PhoneNumber ?? string.Empty,
				OrganizationId = organization.Id.Value,
				CountryCode = user.CountryCode ?? "+1",
				Extension = user.Extension,
				Type = user.PhoneType,
				UserId = appUser.Id
			});
		}

		return new LoginResponse()
		{
			UserName = appUser.UserName,
			Message = $"Welcome to Crit! Enjoy your stay, {appUser.UserName}!"
		};
	}

	public async Task<LoginResponse> LoginAsync(CreateUserRequest user, bool? useCookies)
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
			SignInResult result = await _signInManager.PasswordSignInAsync(applicationUser, user.Password ?? string.Empty, isPersistent, false);

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
