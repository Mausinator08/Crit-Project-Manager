using System.Data.Common;
using Crit.Application.Mappers;
using Crit.Application.RepositoryInterfaces;
using Crit.Contracts.Emails;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Crit.Domain.Identity;
using Crit.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Crit.Application.Emails;

public class EmailService : IEmailService
{
	private readonly IEmailRepository _emailRepository;
	private readonly IMapperService _mapperService;
	private readonly UserManager<ApplicationUser> _userManager;

	public EmailService(IEmailRepository emailRepository, IMapperService mapperService, UserManager<ApplicationUser> userManager)
	{
		_emailRepository = emailRepository;
		_mapperService = mapperService;
		_userManager = userManager;
	}

	public async Task<EmailResponse> GetEmailById(Guid emailId)
	{
		return _mapperService.ConvertTo<Email, EmailResponse>(await _emailRepository.GetEmailById(emailId));
	}

	public async Task<EmailResponse> CreateEmail(CreateEmailRequest email)
	{
		if ((await _emailRepository.GetEmailByAddress(email.EmailAddress)) != null)
		{
			throw new InvalidOperationException($"Failed to create email. The email {email.EmailAddress} already exists.");
		}

		Email createdEmail = await _emailRepository.CreateEmail(_mapperService.ConvertTo<CreateEmailRequest, Email>(email));

		return _mapperService.ConvertTo<Email, EmailResponse>(createdEmail);
	}

	public async Task<EmailResponse> UpdateEmail(Guid emailId, UpdateEmailRequest email)
	{
		Email emailToUpdate = await _emailRepository.GetEmailById(emailId);

		emailToUpdate.EmailAddress = email.EmailAddress ?? emailToUpdate.EmailAddress;
		emailToUpdate.OrganizationId = email.OrganizationId ?? emailToUpdate.OrganizationId;
		emailToUpdate.UserId = email.UserId ?? emailToUpdate.UserId;

		Email updatedEmail = await _emailRepository.UpdateEmail(emailToUpdate);

		return _mapperService.ConvertTo<Email, EmailResponse>(updatedEmail);
	}

	public async Task DeleteEmail(Guid emailId)
	{
		await _emailRepository.DeleteEmail(emailId);
	}
}
