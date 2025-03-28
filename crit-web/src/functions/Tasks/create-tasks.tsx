import { JSX, useState } from "react";
import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { Link } from "../../constants/nav-bar-links";
import { Task } from "../../models/task.model";
import { Status } from "../../models/status.model";
import CollapsibleTask from "../../components/CollapsibleTask/collapsible-task.component";
import UserSelect from "../../components/UserSelect/user-select.component";
import StatusSelect from "../../components/StatusSelect/status-select.component";
import { faClipboardCheck } from "@fortawesome/free-solid-svg-icons";

export function CreateTasks(
    task: Task,
    statuses: Status[],
    isSubtask?: boolean,
): JSX.Element {
    const [assignedUser, setAssignedUser] = useState('Not Assigned');
    const [status, setStatus] = useState('Status Not Set')

    function onAssignedUserSelect(value: string | null): void {
        setAssignedUser(value ?? 'Not Assigned');
    }

    function onStatusSelect(value: string | null): void {
        setStatus(value ?? 'Status Not Set');
    }

    function TaskBody(): JSX.Element {
        return (
            <>
                <td>
                    <UserSelect assignedUser={assignedUser} onAssignedUserSelect={onAssignedUserSelect} taskId={task.id!} />
                </td>
                <td>
                    <StatusSelect statuses={statuses} onStatusSelect={onStatusSelect} taskId={task.id!} status={status} />
                </td>
            </>
        );
    }

    return (() => {
        if (task.subTasks && task.subTasks.length > 0) {
            return (
                <CollapsibleTask key={task.id} task={task} statuses={statuses} isSubtask={isSubtask}>
                    <TaskBody />
                </CollapsibleTask>
            );
        }

        return (
            <tr key={task.id}>
                <td>
                    {task.title}
                </td>
                <TaskBody />
                <td>
                    <h4>
                        <NavLink to={`Projects/${task.projectId}/${task.id}`} key={task.id + '-icon'}>
                            <FontAwesomeIcon icon={faClipboardCheck} />
                        </NavLink>
                    </h4>
                </td>
            </tr>
        );
    })();
}