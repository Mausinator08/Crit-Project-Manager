import { Project } from "./project.model";
import { Task } from "./task.model";

export class Comment {
	constructor(projectId?: string, taskId?: string, text?: string, comment?: any) {
		this.id = comment?.id ?? null;
		this.text = comment?.text ?? text;
		this.taskId = comment?.taskId ?? taskId;
		this.projectId = comment?.projectId ?? projectId;
		this.task = comment?.task ?? null;
		this.project = comment?.project ?? null;
	}

	public id?: string;
	public text: string;
	public projectId: string;
	public taskId: string;
	public task?: Task;
	public project?: Project;
}