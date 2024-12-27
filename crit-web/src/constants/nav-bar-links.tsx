import { icon } from "@fortawesome/fontawesome-svg-core/import.macro";
import { IconDefinition } from "@fortawesome/fontawesome-svg-core";
import { IndexRouteObject, NonIndexRouteObject, RouteObject } from "react-router-dom";

import Home from "../pages/Home/home.page";
import Projects from "../pages/Projects/projects.page";
import ProjectOptions from "../pages/Project/project-options.page";
import Tasks from "../components/Tasks/tasks.component";
import { GetEnvValues } from "./environment";
import { Project } from "../models/project.model";
import { Task } from "../models/task.model";
import TaskDetails from "../components/TaskDetails/task-details.component";

export type Link = { title: string, path: string, icon: IconDefinition, children?: Link[], index?: boolean, showInNavBar?: boolean, data?: any };

const createSubtaskLinks: (task: Task) => Link[] | undefined = (task: Task): Link[] | undefined => {
    return task.subTasks.map<Link>(subTask => {
        return {
            title: subTask?.title ? subTask?.title as string : '<no task title>',
            path: `/Project/${subTask.projectId}/${subTask.id}`,
            icon: icon({ name: 'list-check' }),
            children: createSubtaskLinks(subTask),
            showInNavBar: false,
        };
    });
};

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
