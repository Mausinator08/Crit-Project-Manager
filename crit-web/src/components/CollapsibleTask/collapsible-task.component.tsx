import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Children, JSX, useContext } from "react";

import './collapsible-task.scss';
import { Task } from "../../models/task.model";
import { Status } from "../../models/status.model";
import { ListTasks } from "../../functions/Tasks/list-tasks";
import Collapsible from "../Collapsible/collapsible.component";
import Error from "../../pages/Error/error.page";
import { Table } from "react-bootstrap";
import { ThemeContext } from "../../contexts/Theme/theme-context";
import { faClipboardCheck } from "@fortawesome/free-solid-svg-icons";
import { User } from "../../models/user.model";
import { Priority } from "../../models/priority.model";
import { CustomFieldType } from "../../models/custom-field-type.model";

type CollapsibleTaskItemProps = {
    task: Task,
    children?: React.ReactNode,
    users: User[],
    statuses: Status[],
    priorities: Priority[],
    customFieldTypes: CustomFieldType[],
    hiddenCustomFieldTypeIds: string[],
    isSubtask?: boolean,
};

type CollapsibleTaskBodyProps = {
    task: Task,
    children?: React.ReactNode,
    users: User[],
    statuses: Status[],
    priorities: Priority[],
    customFieldTypes: CustomFieldType[],
    hiddenCustomFieldTypeIds: string[],
}

function CollapsibleTask(props: CollapsibleTaskItemProps): JSX.Element {
    const { theme } = useContext(ThemeContext);

    function CollapsibleTaskBody(props: CollapsibleTaskBodyProps): JSX.Element {
        return (
            <Collapsible key={props.task.id}>
                <Collapsible.Label>
                    <tr>
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
                                        return ListTasks(child, props.users, props.statuses, props.priorities, props.customFieldTypes, props.hiddenCustomFieldTypeIds, true);
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
                <CollapsibleTaskBody task={props.task} users={props.users} statuses={props.statuses} priorities={props.priorities} customFieldTypes={props.customFieldTypes} hiddenCustomFieldTypeIds={props.hiddenCustomFieldTypeIds}>
                    {props.children}
                </CollapsibleTaskBody>
            </>
        );
    } else {
        return (
            <Table responsive striped bordered hover variant={theme}>
                <tbody>
                    <CollapsibleTaskBody task={props.task} users={props.users} statuses={props.statuses} priorities={props.priorities} customFieldTypes={props.customFieldTypes} hiddenCustomFieldTypeIds={props.hiddenCustomFieldTypeIds}>
                        {props.children}
                    </CollapsibleTaskBody>
                </tbody>
            </Table>
        );
    }
}

export default CollapsibleTask;