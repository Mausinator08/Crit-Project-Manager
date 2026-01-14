using Crit.Contracts.ResponseModels;
using Crit.Domain.Models;

namespace Crit.Application.Mappers;

public class OrganizationToOrganizationResponseMapper : IMapper<Organization, OrganizationResponse>
{
	private readonly IMapperService _mapperService;

	public OrganizationToOrganizationResponseMapper(IMapperService mapperService)
	{
		_mapperService = mapperService;
	}
	public OrganizationResponse ConvertTo(Organization fromModel)
	{
		return new OrganizationResponse()
		{
			Id = fromModel.Id,
			Emails = _mapperService.ConvertListTo<Email, EmailResponse>(fromModel.Emails),
			Name = fromModel.Name,
			OrganizationAdmins = _mapperService.ConvertListTo<OrganizationAdmin, OrganizationAdminResponse>(fromModel.OrganizationAdmins),
			OrganizationMembers = _mapperService.ConvertListTo<OrganizationMember, OrganizationMemberResponse>(fromModel.OrganizationMembers),
			OrganizationAffiliates = _mapperService.ConvertListTo<OrganizationAffiliate, OrganizationAffiliateResponse>(fromModel.OrganizationAffiliates),
			OwnerUserId = fromModel.OwnerUserId,
			PhoneNumbers = _mapperService.ConvertListTo<PhoneNumber, PhoneNumberResponse>(fromModel.PhoneNumbers),
			Projects = _mapperService.ConvertListTo<Project, ProjectResponse>(fromModel.Projects)
		};
	}
}
