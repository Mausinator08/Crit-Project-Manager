export class CustomField {
    constructor(customFieldTypeId?: string, name?: string, value?: string, taskId?: string, customField?: any) {
        this.id = customField?.id ?? null;
        this.customFieldTypeId = customField?.customFieldTypeId ?? customFieldTypeId;
        this.value = customField?.value ?? value;
        this.taskId = customField?.taskId ?? taskId;
    }

    public id?: string;
    public customFieldTypeId: string;
    public value?: string;
    public taskId: string;
}