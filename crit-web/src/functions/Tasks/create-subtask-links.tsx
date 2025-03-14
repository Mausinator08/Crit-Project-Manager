import { faListCheck } from "@fortawesome/free-solid-svg-icons";

import { Link } from "../../constants/nav-bar-links";
import { Task } from "../../models/task.model";

export function createSubtaskLinks(task: Task): Link[] | undefined {
    return task.subTasks.map<Link>(subTask => {
        return {
            title: subTask?.title ? subTask?.title as string : '<no task title>',
            path: `/Project/${subTask.projectId}/${subTask.id}`,
            icon: faListCheck,
            children: createSubtaskLinks(subTask),
            showInNavBar: false,
        };
    });
};