export class Organization {
    constructor(name?: string, ownerUserId?: string, organization?: any) {
        this.id = organization?.id ?? crypto.randomUUID();
        this.name = name ?? organization?.name ?? '';
        this.ownerUserId = ownerUserId ?? organization?.ownerUserId ?? '';
        this.phoneNumberIds = organization?.phoneNumberIds ?? [];
        this.emailIds = organization?.emailIds ?? [];
        this.projectIds = organization?.projectIds ?? [];
        this.adminUserIds = organization?.adminUserIds ?? [];
        this.memberUserIds = organization?.memberUserIds ?? [];
        this.affiliatedUserIds = organization?.affiliatedUserIds ?? [];
    }

    public id: string;
    public name: string;
    public ownerUserId: string;
    public phoneNumberIds: string[];
    public emailIds: string[];
    public projectIds: string[];
    public adminUserIds: string[];
    public memberUserIds: string[];
    public affiliatedUserIds: string[];
}