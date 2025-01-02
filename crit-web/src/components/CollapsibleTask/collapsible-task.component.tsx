import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Children, JSX, useContext } from "react";

import { Link } from "../../constants/nav-bar-links";
import './collapsible-task.scss';
import { Task } from "../../models/task.model";
import { Status } from "../../models/status.model";
import { CreateTasks } from "../../functions/Tasks/create-tasks";
import Collapsible from "../Collapsible/collapsible.component";
import Error from "../../pages/Error/error.page";
import { Table } from "react-bootstrap";
import { ThemeContext } from "../../contexts/Theme/theme-context";

type CollapsibleTaskItemProps = {
    link: Link,
    task: Task,
    children?: React.ReactNode,
    statuses: Status[],
    isSubtask?: boolean,
};

type CollapsibleTaskBodyProps = {
    link: Link,
    task: Task,
    children?: React.ReactNode,
    statuses: Status[],
}

function CollapsibleTask(props: CollapsibleTaskItemProps): JSX.Element {
    const link: Link = props.link;
    const { theme } = useContext(ThemeContext);

    function CollapsibleTaskBody(props: CollapsibleTaskBodyProps): JSX.Element {
        return (
            <Collapsible key={link.path}>
                <Collapsible.Label>
                    <tr>
                        <td>
                            {link.title}
                        </td>
                        {props.children}
                        <td>
                            <Collapsible.Toggle>
                                <h4>
                                    <NavLink to={link.path} key={link.path + '-icon'}>
                                        <FontAwesomeIcon icon={link.icon} />
                                    </NavLink>
                                </h4>
                            </Collapsible.Toggle>
                        </td>
                    </tr>
                </Collapsible.Label>
                <tr>
                    <td colSpan={6 + Children.count(props.children) + 1}>
                        <Collapsible.Body>
                            {(() => {
                                if (link.children && link.children.length > 0) {
                                    return link.children.map((child) => {
                                        const task: Task | undefined = props.task.subTasks.find(subTask => subTask.id === (child?.data as Task | undefined)?.id);
                                        if (task) {
                                            return CreateTasks(child, task, props.statuses, true);
                                        } else {
                                            return (
                                                <Error>
                                                    <h6 className='error-text'>Error Loading Task Child!</h6>
                                                    <p className='error-text'>The task child item {link.title} could not be found.</p>
                                                </Error>
                                            );
                                        }
                                    });
                                } else {
                                    return (
                                        <Error>
                                            <h6 className='error-text'>Error Loading Task Children!</h6>
                                            <p className='error-text'>The task item {link.title} could not load its children.</p>
                                        </Error>
                                    );
                                }
                            })()}
                        </Collapsible.Body>
                    </td>
                </tr>
            </Collapsible>
        );
    }

    if (!props.isSubtask || (props.isSubtask && props.isSubtask.valueOf() === false)) {
        return (
            <>
                <CollapsibleTaskBody link={props.link} task={props.task} statuses={props.statuses}>
                    {props.children}
                </CollapsibleTaskBody>
            </>
        );
    } else {
        return (
            <Table responsive striped bordered hover variant={theme}>
                <tbody>
                    <CollapsibleTaskBody link={props.link} task={props.task} statuses={props.statuses}>
                        {props.children}
                    </CollapsibleTaskBody>
                </tbody>
            </Table>
        );
    }
}

export default CollapsibleTask;