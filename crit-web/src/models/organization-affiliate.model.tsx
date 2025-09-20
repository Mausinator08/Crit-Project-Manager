import { Organization } from "./organization.model";

export class OrganizationAffiliate {
	constructor(organizationId: string, affiliateUserId: string, organizationAffiliate?: any) {
		this.id = organizationAffiliate?.id ?? null;
		this.organizationId = organizationAffiliate?.organizationId ?? organizationId;
		this.affiliateUserId = organizationAffiliate?.affiliateUserId ?? affiliateUserId;
		this.organization = organizationAffiliate?.organization ?? null;
	}

	public id?: string;
	public organizationId: string;
	public affiliateUserId: string;
	public organization?: Organization;
}