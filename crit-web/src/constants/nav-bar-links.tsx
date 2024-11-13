import { icon } from "@fortawesome/fontawesome-svg-core/import.macro";
import { IconDefinition } from "@fortawesome/fontawesome-svg-core";
import { IndexRouteObject, NonIndexRouteObject, RouteObject } from "react-router-dom";

import Home from "../pages/Home/home.page";
import Projects from "../pages/Projects/projects.page";
import { GetEnvValues } from "./environment";
import { Project } from "../models/project.model";
import Tasks from "../pages/Tasks/tasks.page";
import ProjectOptions from "../pages/Project/project-options.page";

export type Link = { title: string, path: string, icon: IconDefinition, element: JSX.Element, children?: Link[], index?: boolean };

export const links: Link[] = [
    {
        title: 'Home',
        path: "/",
        icon: icon({ name: 'house' }),
        element: (<Home />),
    },
    {
        title: "Projects",
        path: "/Projects",
        icon: icon({ name: 'list-check' }),
        element: (<Projects />),
        index: true,
        children: await (async (): Promise<Link[]> => {
            return new Promise<Link[]>(async (resolve, reject): Promise<void> => {
                const response = await fetch(new URL(`${GetEnvValues()?.critApiUrl}/Project/GetAllProjects`), {
                    method: 'GET',
                });
                const data: Project[] = await response.json();
                resolve(data.map<Link>(project => { return { title: project.name, path: `/Project/${project.id}`, icon: icon({ name: 'list-check' }), element: (<ProjectOptions />) } }));
            });
        })()
    }
];

export function LinkChildren(link: Link): RouteObject {
    if (link.index === true) {
        return {
            path: link.path,
            element: link.element,
            children: link.children && link.children.length > 0 ? link.children.map<RouteObject>(LinkChildren) : undefined,
            index: true,
        } as IndexRouteObject;
    }

    return {
        path: link.path,
        element: link.element,
        children: link.children && link.children.length > 0 ? link.children.map<RouteObject>(LinkChildren) : undefined,
    } as NonIndexRouteObject;
}
