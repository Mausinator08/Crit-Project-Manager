import { CustomField } from "./custom-field.model";
import { Project } from "./project.model";

export class CustomFieldType
{
    constructor(name?: string, projectId?: string, customFieldType?: any)
    {
        this.id = customFieldType?.id ?? null;
        this.name = customFieldType?.name ?? name;
        this.projectId = customFieldType?.projectId ?? projectId;
        this.hiddenProjectId = customFieldType?.hiddenProjectId ?? null;
        this.project = customFieldType?.project ?? null;
        this.hiddenProject = customFieldType?.hiddenProject ?? null;
        this.customFields = customFieldType?.customFields ?? [];
    }

    public id?: string;
    public name: string;
    public projectId: string;
    public hiddenProjectId?: string;
    public project?: Project;
    public hiddenProject?: Project;
    public customFields?: CustomField[];
}