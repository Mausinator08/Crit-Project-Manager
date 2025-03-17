export class ProjectRequest {
    public name: string = "";
    public description?: string = "";
    public owningOrganizationId?: string = "";
    public organizationIds?: string[] = [];
    public projectUserIds?: string[] = [];
    public projectAdminUserIds?: string[] = [];
    public projectOwnerUserId?: string = "";
}