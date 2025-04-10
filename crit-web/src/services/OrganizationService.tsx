import { GetEnvValues } from "../constants/environment";
import { Organization } from "../models/organization.model";

export class OrganizationService {
    private readonly organizationUrl: string =
        GetEnvValues()?.critApiUrl + "/Organization";

    private constructor() { }

    public async GetAllOrganizations(): Promise<Organization[]> {
        return new Promise<Organization[]>(async (resolve, reject) => {
            const response = await fetch(new URL(this.organizationUrl), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const organizations: Organization[] = await response.json();
            resolve(organizations);
        });
    }

    public async GetAllOrganizationsForProjectId(projectId: string): Promise<Organization[]> {
        return new Promise<Organization[]>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.organizationUrl}/GetAllOrganizationsForProjectId/${projectId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const organizations: Organization[] = await response.json();
            resolve(organizations);
        });
    }

    public async GetAllOrganizationsForUserId(userId: string): Promise<Organization[]> {
        return new Promise<Organization[]>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.organizationUrl}/GetAllOrganizationsForUserId/${userId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const organizations: Organization[] = await response.json();
            resolve(organizations);
        });
    }

    public async GetOrganizationByUserId(userId: string): Promise<Organization> {
        return new Promise<Organization>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.organizationUrl}/GetOrganizationByUserId/${userId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const organization: Organization = await response.json();
            resolve(organization);
        });
    }

    public async GetOrganization(organizationId: string): Promise<Organization> {
        return new Promise<Organization>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.organizationUrl}/${organizationId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const organization: Organization = await response.json();
            resolve(organization);
        });
    }

    public async CreateOrganization(organization: Organization): Promise<Organization> {
        return new Promise<Organization>(async (resolve, reject) => {
            const response = await fetch(new URL(this.organizationUrl), {
                method: "POST",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(organization),
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const newOrganization: Organization = await response.json();
            resolve(newOrganization);
        });
    }

    public async UpdateOrganization(organization: Organization): Promise<void> {
        return new Promise<void>(async (resolve, reject) => {
            const response = await fetch(new URL(this.organizationUrl), {
                method: "PUT",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(organization),
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            resolve();
        });
    }

    public async DeleteOrganization(organizationId: string): Promise<void> {
        return new Promise<void>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.organizationUrl}/${organizationId}`), {
                method: "DELETE",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            resolve();
        });
    }
}