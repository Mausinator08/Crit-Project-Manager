import { Organization } from "./organization.model";

export class OrganizationMember {
	constructor(organizationId: string, memberUserId: string, organizationMember?: any) {
		this.id = organizationMember?.id ?? null;
		this.organizationId = organizationMember?.organizationId ?? organizationId;
		this.memberUserId = organizationMember?.memberUserId ?? memberUserId;
		this.organization = organizationMember?.organization ?? null;
	}

	public id?: string;
	public organizationId: string;
	public memberUserId: string;
	public organization?: Organization;
}