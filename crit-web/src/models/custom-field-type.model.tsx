export class CustomFieldType {
    constructor(name?: string, projectId?: string, customFieldType?: any) {
        this.id = customFieldType?.id ?? null;
        this.name = customFieldType?.name ?? name;
        this.projectId = customFieldType?.projectId ?? projectId;
    }

    public id?: string;
    public name: string;
    public projectId: string;
}