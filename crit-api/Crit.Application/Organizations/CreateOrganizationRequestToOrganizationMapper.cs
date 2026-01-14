using Crit.Contracts.RequestModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class CreateOrganizationRequestToOrganizationMapper : IMapper<CreateOrganizationRequest, Organization>
{
	private readonly IMapperService _mapperService;
	public CreateOrganizationRequestToOrganizationMapper(IMapperService mapperService)
	{
		_mapperService = mapperService;
	}

	public Organization ConvertTo(CreateOrganizationRequest fromModel)
	{
		return new Organization()
		{
			Name = fromModel.Name,
			Emails = new List<Email>([
				new Email()
				{
					EmailAddress = fromModel.Email,
					UserId = fromModel.OwnerUserId
				}
			]),
			PhoneNumbers = new List<PhoneNumber>([
				new PhoneNumber()
				{
					CountryCode = fromModel.CountryCode,
					Extension = fromModel.Extension,
					Number = fromModel.PhoneNumber,
					Type = fromModel.NumberType,
					UserId = fromModel.OwnerUserId
				}
			]),
			OwnerUserId = fromModel.OwnerUserId,
			OrganizationAdmins = fromModel.OrganizationAdminIds.Select(oa => new OrganizationAdmin()
			{
				AdminUserId = oa
			}).ToList(),
			OrganizationMembers = fromModel.OrganizationMemberIds.Select(om => new OrganizationMember()
			{
				MemberUserId = om
			}).ToList(),
			OrganizationAffiliates = fromModel.OrganizationAffiliateIds.Select(oaf => new OrganizationAffiliate()
			{
				AffiliateUserId = oaf
			}).ToList()
		};
	}
}
