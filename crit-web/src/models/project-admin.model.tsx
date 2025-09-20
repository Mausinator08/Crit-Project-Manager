import { Project } from "./project.model";

export class ProjectAdmin {
	constructor(projectId: string, adminId: string, projectAdmin?: any) {
		this.id = projectAdmin?.id ?? null;
		this.projectId = projectAdmin.projectId ?? projectId;
		this.adminId = projectAdmin.adminId ?? adminId;
		this.project = projectAdmin.project ?? null;
	}

	public id?: string;
	public projectId: string;
	public adminId: string;
	public project?: Project;
}