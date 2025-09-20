import { Email } from "./email.model";
import { OrganizationAdmin } from "./organization-admin.model";
import { OrganizationAffiliate } from "./organization-affiliate.model";
import { OrganizationMember } from "./organization-member.model";
import { OrganizationProject } from "./organization-project.model";
import { PhoneNumber } from "./phone-number.model";
import { Project } from "./project.model";

export class Organization {
    constructor(name?: string, ownerUserId?: string, organization?: any) {
        this.id = organization?.id ?? null;
        this.name = name ?? organization?.name ?? '';
        this.ownerUserId = ownerUserId ?? organization?.ownerUserId ?? '';
        this.phoneNumbers = organization?.phoneNumbers ?? [];
        this.emails = organization?.emails ?? [];
        this.projects = organization?.projects ?? [];
        this.organizationProjects = organization?.organizationProjects ?? [];
        this.organizationAdmins = organization?.organizationAdmins ?? [];
        this.organizationMembers = organization?.organizationMembers ?? [];
        this.organizationAffiliates = organization?.organizationAffiliates ?? [];
    }

    public id?: string;
    public name: string;
    public ownerUserId: string;
    public phoneNumbers: PhoneNumber[];
    public emails: Email[];
    public projects: Project[];
    public organizationProjects: OrganizationProject[];
    public organizationAdmins: OrganizationAdmin[];
    public organizationMembers: OrganizationMember[];
    public organizationAffiliates: OrganizationAffiliate[];
}