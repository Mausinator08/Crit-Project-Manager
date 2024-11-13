import { CustomFieldType } from "./custom-field-type.model";
import { Priority } from "./priority.model";
import { Status } from "./status.model";
import { Task } from "./task.model";

export class Project {
    constructor(name?: string, project?: any) {
        this.id = project?.id ?? crypto.randomUUID();
        this.name = project?.name ?? name;
        this.description = project?.description ?? null;
        this.tasks = project?.tasks ?? [];
        this.owningOrganizationId = project?.owningOrganizationId ?? null;
        this.projectOwnerUserId = project?.projectOwnerUserId ?? null;
        this.projectAdminUserIds = project?.projectAdminUserIds ?? [];
        this.projectUserIds = project?.projectUserIds ?? [];
        this.organizationIds = project?.organizationIds ?? [];
        this.customFieldTypes = project?.customFieldTypes ?? [];
        this.statuses = project?.statuses ?? [];
        this.priorities = project?.priorities ?? [];
    }

    public id: string;
    public name: string;
    public description?: string;
    public tasks: Task[];
    public owningOrganizationId?: string;
    public projectOwnerUserId?: string;
    public projectAdminUserIds: string[];
    public projectUserIds: string[];
    public organizationIds: string[];
    public customFieldTypes: CustomFieldType[];
    public statuses: Status[];
    public priorities: Priority[];
}