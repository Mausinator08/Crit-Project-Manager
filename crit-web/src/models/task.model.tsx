import { CustomField } from "./custom-field.model";

export class Task {
    constructor(projectId?: string, task?: any) {
        this.id = task?.id ?? crypto.randomUUID();
        this.title = task?.title ?? null;
        this.details = task?.details ?? null;
        this.projectId = task?.projectId ?? projectId;
        this.colaboratorUserIds = task?.colaboratorUserIds ?? [];
        this.assignedUserId = task?.assignedUserId ?? null;
        this.statusId = task?.statusId ?? null;
        this.priorityId = task?.priorityId ?? null;
        this.complexity = task?.complexity ?? null;
        this.dueDate = task?.dueDate ?? null;
        this.taskDependencyIds = task?.taskDependencyIds ?? [];
        this.subTasks = task?.subTasks ?? [];
        this.customFields = task?.customFields ?? [];
    }

    public id: string;
    public title?: string;
    public details?: string;
    public projectId: string;
    public colaboratorUserIds: string[];
    public assignedUserId?: string;
    public statusId?: string;
    public priorityId?: string;
    public complexity?: number;
    public dueDate?: Date;
    public taskDependencyIds: string[];
    public subTasks: Task[];
    public customFields: CustomField[];
}