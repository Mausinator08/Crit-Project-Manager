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
import { faClipboardCheck } from "@fortawesome/free-solid-svg-icons";

type CollapsibleTaskItemProps = {
    task: Task,
    children?: React.ReactNode,
    statuses: Status[],
    isSubtask?: boolean,
};

type CollapsibleTaskBodyProps = {
    task: Task,
    children?: React.ReactNode,
    statuses: Status[],
}

function CollapsibleTask(props: CollapsibleTaskItemProps): JSX.Element {
    const { theme } = useContext(ThemeContext);

    function CollapsibleTaskBody(props: CollapsibleTaskBodyProps): JSX.Element {
        return (
            <Collapsible key={props.task.id}>
                <Collapsible.Label>
                    <tr>
                        <td>
                            {props.task.title}
                        </td>
                        {props.children}
                        <td>
                            <Collapsible.Toggle>
                                <h4>
                                    <NavLink to={`Projects/${props.task.projectId}/${props.task.id}`} key={props.task.id + '-icon'}>
                                        <FontAwesomeIcon icon={faClipboardCheck} />
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
                                if (props.task.subTasks && props.task.subTasks.length > 0) {
                                    return props.task.subTasks.map((child) => {
                                        return CreateTasks(child, props.statuses, true);
                                    });
                                } else {
                                    return (
                                        <Error>
                                            <h6 className='error-text'>Error Loading Task Children!</h6>
                                            <p className='error-text'>The task item {props.task.title} could not load its children.</p>
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
                <CollapsibleTaskBody task={props.task} statuses={props.statuses}>
                    {props.children}
                </CollapsibleTaskBody>
            </>
        );
    } else {
        return (
            <Table responsive striped bordered hover variant={theme}>
                <tbody>
                    <CollapsibleTaskBody task={props.task} statuses={props.statuses}>
                        {props.children}
                    </CollapsibleTaskBody>
                </tbody>
            </Table>
        );
    }
}

export default CollapsibleTask;