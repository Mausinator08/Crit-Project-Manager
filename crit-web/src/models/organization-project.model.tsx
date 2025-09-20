import { Organization } from "./organization.model";
import { Project } from "./project.model";

export class OrganizationProject {
	constructor(organizationId: string, projectId: string, organizationProject?: any) {
		this.id = organizationProject?.id ?? null;
		this.organizationId = organizationProject?.organizationId ?? organizationId;
		this.projectId = organizationProject?.projectId ?? projectId;
		this.organization = organizationProject?.organization ?? null;
		this.project = organizationProject?.project ?? null;
	}

	public id?: string;
	public organizationId: string;
	public projectId: string;
	public organization?: Organization;
	public project?: Project;
}