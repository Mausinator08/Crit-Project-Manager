import { Organization } from "./organization.model";

export class Email
{
    constructor(userId?: string, emailAddress?: string, organizationId?: string, email?: any)
    {
        this.userId = userId ?? email?.id ?? '';
        this.emailAddress = emailAddress ?? email?.emailAddress;
        this.organizationId = organizationId ?? email?.organizationId ?? '';
        this.id = email?.id ?? null;
        this.organization = email?.organization ?? null;
    }

    public id?: string;
    public userId: string;
    public emailAddress?: string;
    public organizationId: string;
    public organization?: Organization;
}