using Crit.Contracts.Emails;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Emails;

public interface IEmailService
{
	Task<EmailResponse> GetEmailById(Guid emailId);
	Task<EmailResponse> CreateEmail(CreateEmailRequest email);
	Task<EmailResponse> UpdateEmail(Guid emailId, UpdateEmailRequest email);
	Task DeleteEmail(Guid emailId);
}
