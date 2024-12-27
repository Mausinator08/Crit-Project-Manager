import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { icon } from "@fortawesome/fontawesome-svg-core/import.macro";

import { Link } from "../../constants/nav-bar-links";

import './collapsible-task-item.scss';
import { Task } from "../../models/task.model";
import { Dropdown } from "react-bootstrap";
import { Status } from "../../models/status.model";
import UserSelect from "../UserSelect/user-select.component";
import StatusSelect from "../StatusSelect/status-select.component";

type CollapsibleTaskItemProps = {
    link: Link,
    task: Task,
    children?: React.ReactNode,
    statuses: Status[],
};

export function CreateLinks(
    link: Link,
    task: Task,
    statuses: Status[]
): JSX.Element {
    const [assignedUser, setAssignedUser] = useState('Not Assigned');
    const [status, setStatus] = useState('Status Not Set')

    const onAssignedUserSelect: (value: string | null) => void = (value: string | null): void => {
        setAssignedUser(value ?? 'Not Assigned');
    }

    const onStatusSelect: (value: string | null) => void = (value: string | null): void => {
        setStatus(value ?? 'Status Not Set');
    }

    return (() => {
        if (link.children && link.children.length > 0) {
            return (
                <tr>
                    <CollapsibleTaskItem key={link.path} link={link} task={task} statuses={statuses}>
                        <td>
                            <Dropdown key={link.path}>
                                <h5>Not Assigned</h5>
                            </Dropdown>
                        </td>
                    </CollapsibleTaskItem>
                </tr>
            );
        }

        return (
            <tr key={link.path}>
                <td>
                    <h4>
                        <NavLink to={link.path} key={link.path + '-icon'}>
                            <FontAwesomeIcon icon={link.icon} />
                        </NavLink>
                        <NavLink to={link.path} key={link.path + '-text'}>
                            <h5>{link.title}</h5>
                        </NavLink>
                    </h4>
                </td>
                <td>
                    <UserSelect assignedUser={assignedUser} onAssignedUserSelect={onAssignedUserSelect} path={link.path} />
                </td>
                <td>
                    <StatusSelect statuses={statuses} onStatusSelect={onStatusSelect} path={link.path} />
                </td>
            </tr>
        );
    })();
}

function CollapsibleTaskItem(props: CollapsibleTaskItemProps): JSX.Element {
    const link: Link = props.link;
    const [collapsed, setCollapsed] = useState<boolean>(true);

    function toggleAccordion(event: React.MouseEvent<HTMLElement>) {
        if (collapsed === true) {
            setCollapsed(false);
        } else {
            setCollapsed(true);
        }
    }

    return (
        <div key={link.path}>
            <h4 onClick={toggleAccordion}>
                <NavLink to={link.path} key={link.path + '-icon'}>
                    <FontAwesomeIcon icon={link.icon} />
                </NavLink>
                <NavLink to={link.path} key={link.path + '-text'}>
                    <h5>{link.title}</h5>
                </NavLink>
                <button className="toggle-folding-button" onClick={toggleAccordion}><FontAwesomeIcon icon={collapsed === true ? icon({ name: 'chevron-down' }) : icon({ name: 'chevron-up' })} /></button>
            </h4>
            <div className={collapsed === true ? 'folded' : 'expanded'}>
                {(() => {
                    if (link.children && link.children.length > 0) {
                        return link.children.map((child) => {
                            const task: Task | undefined = props.task.subTasks.find(subTask => subTask.id === (child?.data as Task | undefined)?.id);
                            if (task) {
                                return CreateLinks(child, task, props.statuses);
                            } else {
                                return (
                                    <div>
                                        <h6 className='error-text'>Error Loading Task Child!</h6>
                                        <p className='error-text'>The task child item {link.title} could not be found.</p>
                                    </div>
                                );
                            }
                        });
                    } else {
                        return (
                            <div>
                                <h6 className='error-text'>Error Loading Task Children!</h6>
                                <p className='error-text'>The task item {link.title} could not load its children.</p>
                            </div>
                        );
                    }
                })()}
            </div>
        </div>
    );
}

export default CollapsibleTaskItem;