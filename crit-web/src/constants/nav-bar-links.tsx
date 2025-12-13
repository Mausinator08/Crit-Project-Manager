import { faHouse, faBarsProgress, faCheck, faSignInAlt, faSignOutAlt, faRegistered } from "@fortawesome/free-solid-svg-icons";
import { IconDefinition } from "@fortawesome/fontawesome-svg-core";

import { GetEnvValues } from "./environment";
import { Project } from "../models/project.model";

export type Link = { title: string, path: string, icon: IconDefinition, children?: Link[], index?: boolean, roles?: string[], data?: any };

export const GetLinks = async () => {
    return new Promise<Link[]>(async (resolve, reject): Promise<void> => {
        const links: Link[] = [
            {
                title: 'Home',
                path: "/Home",
                icon: faHouse,
                roles: ['OrganizationOwner', 'OrganizationAdmin', 'ProjectOwner', 'ProjectAdmin', 'User'],
            },
            {
                title: "Projects",
                path: "/Projects",
                icon: faBarsProgress,
                roles: ['OrganizationOwner', 'OrganizationAdmin', 'ProjectOwner', 'ProjectAdmin', 'User'],
                index: true,
                children: await (async (): Promise<Link[]> => {
                    return new Promise<Link[]>(async (resolve, reject): Promise<void> => {
                        const response = await fetch(new URL(`${GetEnvValues()?.critApiUrl}/Project`), {
                            method: 'GET',
                            mode: 'cors',
                            credentials: 'include',
                        });
                        const data: Project[] = await response.json();
                        resolve(data.map<Link>(project => {
                            return {
                                title: project.name,
                                path: `/Projects/${project.id}`,
                                icon: faCheck,
                            };
                        }));
                    })
                })(),
            },
            {
                title: 'Logout',
                path: "/Logout",
                icon: faSignOutAlt,
            }
        ];
        resolve(links);
    });
};


export const GetLoggedOutLinks = async () => {
    return new Promise<Link[]>(async (resolve, reject): Promise<void> => {
        const links: Link[] = [
            {
                title: 'Login',
                path: "/Login",
                icon: faSignInAlt,
            },
            {
                title: 'Register',
                path: '/Register',
                icon: faRegistered,
            }
        ];
        resolve(links);
    });
};
