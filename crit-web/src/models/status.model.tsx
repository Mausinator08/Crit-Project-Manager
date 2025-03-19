export class Status {
    constructor(name?: string, projectId?: string, status?: any) {
        this.id = status?.id ?? null;
        this.name = status?.name ?? name;
        this.description = status?.description ?? null;
        this.backgroundColor = status?.backgroundColor ?? null;
        this.color = status?.color ?? null;
        this.projectId = status?.projectId ?? projectId;
        this.taskId = status?.taskId ?? null;
    }

    public id?: string;
    public name: string;
    public description?: string;
    public backgroundColor?: string;
    public color?: string;
    public projectId: string;
    public taskId?: string;
}