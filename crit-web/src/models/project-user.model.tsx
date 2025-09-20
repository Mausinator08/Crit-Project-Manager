import { Project } from "./project.model";

export class ProjectUser {
	constructor(projectId: string, userId: string, projectUser?: any) {
		this.id = projectUser?.id ?? null;
		this.projectId = projectUser.projectId ?? projectId;
		this.userId = projectUser.userId ?? userId;
		this.project = projectUser.project ?? null;
	}

	public id?: string;
	public projectId: string;
	public userId: string;
	public project?: Project;
}