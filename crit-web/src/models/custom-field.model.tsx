export class CustomField {
    constructor(customFieldTypeId?: string, name?: string, value?: any, taskId?: string, customField?: any) {
        this.id = customField?.id ?? null;
        this.customFieldTypeId = customField?.customFieldTypeId ?? customFieldTypeId;
        this.name = customField?.name ?? name;
        this.value = customField?.value ?? value;
        this.taskId = customField?.taskId ?? taskId;
    }

    public id?: string;
    public customFieldTypeId: string;
    public name?: string;
    public value?: any;
    public taskId: string;
}