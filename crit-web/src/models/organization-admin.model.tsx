import { Organization } from "./organization.model";

export class OrganizationAdmin {
	constructor(organizationId: string, adminUserId: string, organizationAdmin?: any) {
		this.id = organizationAdmin?.id ?? null;
		this.organizationId = organizationAdmin?.organizationId ?? organizationId;
		this.adminUserId = organizationAdmin?.adminUserId ?? adminUserId;
		this.organization = organizationAdmin?.organization ?? null;
	}

	public id?: string;
	public organizationId: string;
	public adminUserId: string;
	public organization?: Organization;
}