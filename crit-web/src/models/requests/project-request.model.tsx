export class ProjectRequest
{
    public name: string = "";
    public description?: string = "";
    public owningOrganizationId?: string | null = null;
    public organizationIds?: string[] = [];
    public projectUserIds?: string[] = [];
    public projectAdminUserIds?: string[] = [];
    public projectOwnerUserId?: string | null = null;
}