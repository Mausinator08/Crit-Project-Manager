import { Organization } from "../models/organization.model";

export const USER_ROLES = {
    User: { value: 'User', display: 'User' },
    ProjectAdmin: { value: 'ProjectAdmin', display: 'Project Admin' },
    ProjectOwner: { value: 'ProjectOwner', display: 'Project Owner' },
    OrganizationAdmin: { value: 'OrganizationAdmin', display: 'Organization Admin' },
    OrganizationOwner: { value: 'OrganizationOwner', display: 'Organization Owner' },
};