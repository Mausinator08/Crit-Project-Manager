import { Project } from "./project.model";
import { Task } from "./task.model";

export class Priority {
    constructor(name?: string, projectId?: string, priority?: any) {
        this.id = priority?.id ?? null;
        this.name = priority?.name ?? name;
        this.description = priority?.description ?? null;
        this.backgroundColor = priority?.backgroundColor ?? null;
        this.color = priority?.color ?? null;
        this.projectId = priority?.projectId ?? projectId;
        this.taskId = priority?.taskId ?? null;
        this.project = priority?.project ?? null;
        this.tasks = priority?.tasks ?? null;
    }

    public id?: string;
    public name: string;
    public description?: string;
    public backgroundColor?: string;
    public color?: string;
    public projectId: string;
    public taskId?: string;
    public project?: Project;
    public tasks?: Task[];
}