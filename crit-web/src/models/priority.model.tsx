export class Priority {
    constructor(name?: string, projectId?: string, priority?: any) {
        this.id = priority?.id ?? crypto.randomUUID();
        this.name = priority?.name ?? name;
        this.description = priority?.description ?? null;
        this.backgroundColor = priority?.backgroundColor ?? null;
        this.color = priority?.color ?? null;
        this.projectId = priority?.projectId ?? projectId;
    }

    public id: string;
    public name: string;
    public description?: string;
    public backgroundColor?: string;
    public color?: string;
    public projectId: string;
}