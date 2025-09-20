import { CustomFieldType } from "./custom-field-type.model";
import { Task } from "./task.model";

export class CustomField
{
    constructor(customFieldTypeId?: string, name?: string, value?: string, taskId?: string, customField?: any)
    {
        this.id = customField?.id ?? null;
        this.customFieldTypeId = customField?.customFieldTypeId ?? customFieldTypeId;
        this.value = customField?.value ?? value;
        this.taskId = customField?.taskId ?? taskId;
        this.task = customField?.task ?? null;
        this.customFieldType = customField?.customFieldType ?? null;
    }

    public id?: string;
    public customFieldTypeId: string;
    public value?: string;
    public taskId: string;
    public task?: Task;
    public customFieldType?: CustomFieldType;
}