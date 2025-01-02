import { icon } from "@fortawesome/fontawesome-svg-core/import.macro";
import { IconDefinition } from "@fortawesome/fontawesome-svg-core";

import { GetEnvValues } from "./environment";
import { Project } from "../models/project.model";
import { createSubtaskLinks } from "../functions/Tasks/create-subtask-links";

export type Link = { title: string, path: string, icon: IconDefinition, children?: Link[], index?: boolean, showInNavBar?: boolean, data?: any };

export const links: Link[] = [
    {
        title: 'Home',
        path: "/",
        icon: icon({ name: 'house' }),
    },
    {
        title: "Projects",
        path: "/Projects",
        icon: icon({ name: 'bars-progress' }),
        index: true,
        children: await (async (): Promise<Link[]> => {
            return new Promise<Link[]>(async (resolve, reject): Promise<void> => {
                const response = await fetch(new URL(`${GetEnvValues()?.critApiUrl}/Project/GetAllProjects`), {
                    method: 'GET',
                });
                const data: Project[] = await response.json() as Project[];
                resolve(data.map<Link>(project => {
                    return {
                        title: project.name,
                        path: `/Project/${project.id}`,
                        icon: icon({ name: 'bars-progress' }),
                        children: project.tasks.map<Link>(task => {
                            return {
                                title: task.title ? task.title : '<no task title>',
                                path: `/Project/${project.id}/${task.id}`,
                                icon: icon({ name: 'list-check' }),
                                children: createSubtaskLinks(task),
                                showInNavBar: false,
                                data: task,
                            };
                        })
                    };
                }))
            })
        })(),
    }
];
