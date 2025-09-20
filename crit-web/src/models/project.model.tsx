import { Comment } from "./comment.model";
import { CustomFieldType } from "./custom-field-type.model";
import { OrganizationProject } from "./organization-project.model";
import { Organization } from "./organization.model";
import { Priority } from "./priority.model";
import { ProjectAdmin } from "./project-admin.model";
import { ProjectUser } from "./project-user.model";
import { Status } from "./status.model";
import { Task } from "./task.model";

export class Project {
    constructor(name?: string, project?: any) {
        this.id = project?.id ?? null;
        this.name = project?.name ?? name;
        this.description = project?.description ?? null;
        this.tasks = project?.tasks ?? [];
        this.owningOrganizationId = project?.owningOrganizationId ?? null;
        this.projectOwnerUserId = project?.projectOwnerUserId ?? null;
        this.projectUsers = project?.projectUsers ?? [];
        this.organizations = project?.organizations ?? [];
        this.customFieldTypes = project?.customFieldTypes ?? [];
        this.hiddenCustomFieldTypes = project?.hiddenCustomFieldTypes ?? [];
        this.statuses = project?.statuses ?? [];
        this.priorities = project?.priorities ?? [];
        this.comments = project?.comments ?? [];
        this.organizationProjects = project?.organizationProjects ?? [];
        this.projectUsers = project?.projectUsers ?? [];
        this.projectAdmins = project?.projectAdmins ?? [];
    }

    public id?: string;
    public name: string;
    public description?: string;
    public owningOrganizationId?: string;
    public projectOwnerUserId?: string;
    public owningOrganization?: Organization;
    public statuses: Status[];
    public priorities: Priority[];
    public customFieldTypes: CustomFieldType[];
    public tasks: Task[];
    public organizations: Organization[];
    public hiddenCustomFieldTypes: CustomFieldType[];
    public comments: Comment[];
    public organizationProjects: OrganizationProject[];
    public projectUsers: ProjectUser[];
    public projectAdmins: ProjectAdmin[];
}