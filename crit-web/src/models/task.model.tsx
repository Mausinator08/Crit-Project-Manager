import { CustomField } from "./custom-field.model";
import { Priority } from "./priority.model";
import { Project } from "./project.model";
import { Status } from "./status.model";
import { Comment } from "./comment.model";

export class Task {
    constructor(projectId?: string, task?: any) {
        this.id = task?.id ?? null;
        this.title = task?.title ?? null;
        this.details = task?.details ?? null;
        this.projectId = task?.projectId ?? projectId;
        this.assignedUserId = task?.assignedUserId ?? null;
        this.statusId = task?.statusId ?? null;
        this.priorityId = task?.priorityId ?? null;
        this.complexity = task?.complexity ?? null;
        this.dueDate = task?.dueDate ?? null;
        this.parentTaskId = task?.parentTaskId ?? null;
        this.customFields = task?.customFields ?? [];
        this.project = task?.project ?? null;
        this.status = task?.status ?? null;
        this.priority = task?.priority ?? null;
        this.subTasks = task?.subTasks ?? [];
        this.parentTask = task?.parentTask ?? null;
        this.comments = task?.comments ?? [];
    }

    public id?: string;
    public title?: string;
    public details?: string;
    public projectId: string;
    public assignedUserId?: string;
    public statusId?: string;
    public priorityId?: string;
    public complexity?: number;
    public dueDate?: Date;
    public parentTaskId?: string;
    public customFields: CustomField[];
    public project?: Project;
    public status?: Status;
    public priority?: Priority;
    public subTasks: Task[];
    public parentTask?: Task;
    public comments?: Comment[];
}