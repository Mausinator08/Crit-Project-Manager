export class Email {
    constructor(userId?: string, emailAddress?: string, organizationId?: string, task?: any) {
        this.userId = userId ?? task?.id ?? '';
        this.emailAddress = emailAddress ?? task?.emailAddress;
        this.organizationId = organizationId ?? task?.organizationId ?? '';
        this.id = task?.id ?? crypto.randomUUID();
    }

    public id: string;
    public userId: string;
    public emailAddress?: string;
    public organizationId: string;
}