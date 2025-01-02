import { JSX, useState } from "react";
import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { Link } from "../../constants/nav-bar-links";
import { Task } from "../../models/task.model";
import { Status } from "../../models/status.model";
import CollapsibleTask from "../../components/CollapsibleTask/collapsible-task.component";
import UserSelect from "../../components/UserSelect/user-select.component";
import StatusSelect from "../../components/StatusSelect/status-select.component";

export function CreateTasks(
    link: Link,
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
                    <UserSelect assignedUser={assignedUser} onAssignedUserSelect={onAssignedUserSelect} path={link.path} />
                </td>
                <td>
                    <StatusSelect statuses={statuses} onStatusSelect={onStatusSelect} path={link.path} status={status} />
                </td>
            </>
        );
    }

    return (() => {
        if (link.children && link.children.length > 0) {
            return (
                <CollapsibleTask key={link.path} link={link} task={task} statuses={statuses} isSubtask={isSubtask}>
                    <TaskBody />
                </CollapsibleTask>
            );
        }

        return (
            <tr key={link.path}>
                <td>
                    {link.title}
                </td>
                <TaskBody />
                <td>
                    <h4>
                        <NavLink to={link.path} key={link.path + '-icon'}>
                            <FontAwesomeIcon icon={link.icon} />
                        </NavLink>
                    </h4>
                </td>
            </tr>
        );
    })();
}