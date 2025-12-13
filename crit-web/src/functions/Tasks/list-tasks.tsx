import { JSX, useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { Task } from "../../models/task.model";
import { Status } from "../../models/status.model";
import CollapsibleTask from "../../components/CollapsibleTask/collapsible-task.component";
import UserSelect from "../../components/UserSelect/user-select.component";
import StatusSelect from "../../components/StatusSelect/status-select.component";
import { faClipboardCheck } from "@fortawesome/free-solid-svg-icons";
import { Priority } from "../../models/priority.model";
import { User } from "../../models/requests/user.model";
import PrioritySelect from "../../components/PrioritySelect/priority-select.component";
import ComplexitySelect from "../../components/ComplexitySelect/complexity-select.component";
import DueDateSelect from "../../components/DueDateSelect/due-date-select.component";
import CustomFieldSelect from "../../components/CustomFieldSelect/custom-field-select.component";
import { CustomFieldType } from "../../models/custom-field-type.model";

export function ListTasks(
    task: Task,
    users: User[],
    statuses: Status[],
    priorities: Priority[],
    customFieldTypes: CustomFieldType[],
    hiddenCustomFieldTypes: CustomFieldType[],
    isSubtask?: boolean,
): JSX.Element {
    const [listTasksStates, setListTasksStates] = useState<{
        assignedUser: string | undefined;
        status: string | undefined;
        priority: string | undefined;
        complexity: number | undefined;
        dueDate: Date | undefined;
        customFields: (string | undefined)[];
    }>({
        assignedUser: undefined,
        status: undefined,
        priority: undefined,
        complexity: undefined,
        dueDate: undefined,
        customFields: [],
    });

    function onAssignedUserSelect(value: string | undefined | null): void {
        if (value) {
            setListTasksStates(prevState => ({ ...prevState, assignedUser: value }));
            const user = users.find(u => u.id === value);
            if (user) {
                task.assignedUserId = user.id;
            }
        } else {
            setListTasksStates(prevState => ({ ...prevState, assignedUser: undefined }));
            task.assignedUserId = undefined;
        }
    }

    function onStatusSelect(value: string | undefined | null): void {
        if (value) {
            setListTasksStates(prevState => ({ ...prevState, status: value }));
            const status = statuses.find(s => s.id === value);
            if (status) {
                task.statusId = status.id;
            }
        } else {
            setListTasksStates(prevState => ({ ...prevState, status: undefined }));
            task.statusId = undefined;
        }
    }

    function onPrioritySelect(value: string | undefined | null): void {
        if (value) {
            setListTasksStates(prevState => ({ ...prevState, priority: value }));
            const priority = priorities.find(s => s.id === value);
            if (priority) {
                task.priorityId = priority.id;
            }
        } else {
            setListTasksStates(prevState => ({ ...prevState, priority: undefined }));
            task.priorityId = undefined;
        }
    }

    function onComplexitySelect(value: number | undefined | null): void {
        if (value) {
            setListTasksStates(prevState => ({ ...prevState, complexity: value }));
            task.complexity = value;
        } else {
            setListTasksStates(prevState => ({ ...prevState, complexity: undefined }));
            task.complexity = undefined;
        }
    }

    function onDueDateSelect(value: Date | undefined | null): void {
        if (value) {
            setListTasksStates(prevState => ({ ...prevState, dueDate: value }));
            task.dueDate = value;
        }
        else {
            setListTasksStates(prevState => ({ ...prevState, dueDate: undefined }));
            task.dueDate = undefined;
        }
    }

    function onCustomFieldSelect(value: string | undefined | null): void {
        if (value) {
            task.customFields = task.customFields.map(cf => {
                if (cf.id === value) {
                    cf.value = value;
                }
                return cf;
            });
            setListTasksStates(prevState => ({ ...prevState, customFields: task.customFields.map(cf => cf.id) }));
        } else {
            task.customFields = task.customFields.map(cf => {
                if (cf.id === value) {
                    cf.value = undefined;
                }
                return cf;
            });
            setListTasksStates(prevState => ({ ...prevState, customFields: task.customFields.map(cf => cf.id) }));
        }
    }

    useEffect(() => {
        const updates: Partial<typeof listTasksStates> = {};

        if (task.assignedUserId) {
            const user = users.find(u => u.id === task.assignedUserId);
            updates.assignedUser = user?.id;
        } else {
            updates.assignedUser = undefined;
        }

        if (task.statusId) {
            const status = statuses.find(s => s.id === task.statusId);
            updates.status = status?.id;
        } else {
            updates.status = undefined;
        }

        if (task.priorityId) {
            const priority = priorities.find(s => s.id === task.priorityId);
            updates.priority = priority?.id;
        } else {
            updates.priority = undefined;
        }

        if (task.complexity) {
            updates.complexity = task.complexity;
        } else {
            updates.complexity = undefined;
        }

        if (task.dueDate) {
            updates.dueDate = task.dueDate;
        } else {
            updates.dueDate = undefined;
        }

        updates.customFields = task.customFields.map(cf => cf.id);

        setListTasksStates(prevState => ({ ...prevState, ...updates }));
    }, []);

    function TaskTableRow(): JSX.Element {
        return (
            <>
                <td>
                    {task.title}
                </td>
                <td>
                    <UserSelect users={users} assignedUser={listTasksStates.assignedUser} onAssignedUserSelect={onAssignedUserSelect} taskId={task.id!} />
                </td>
                <td>
                    <StatusSelect statuses={statuses} onStatusSelect={onStatusSelect} taskId={task.id!} status={listTasksStates.status} />
                </td>
                <td>
                    <PrioritySelect priorities={priorities} onPrioritySelect={onPrioritySelect} taskId={task.id!} priority={listTasksStates.priority} />
                </td>
                <td>
                    <ComplexitySelect taskId={task.id!} onComplexitySelect={onComplexitySelect} complexity={listTasksStates.complexity} />
                </td>
                <td>
                    <DueDateSelect taskId={task.id!} dueDate={listTasksStates.dueDate} onDueDateSelect={onDueDateSelect} />
                </td>
                <td>
                    {customFieldTypes.length > 0 &&
                        customFieldTypes.filter(fieldType => !hiddenCustomFieldTypes.find(type => type.id === fieldType.id)).map<JSX.Element>(fieldType =>
                            <>
                                <CustomFieldSelect
                                    taskId={task.id!}
                                    customFields={task.customFields}
                                    customFieldType={fieldType}
                                    onCustomFieldSelect={onCustomFieldSelect}
                                    customField={listTasksStates.customFields.find(cf => cf === fieldType.id)} />
                            </>
                        )
                    }
                </td>
            </>
        );
    }

    return (() => {
        if (task.subTasks && task.subTasks.length > 0) {
            return (
                <CollapsibleTask
                    key={task.id}
                    task={task}
                    users={users}
                    statuses={statuses}
                    priorities={priorities}
                    isSubtask={isSubtask}
                    customFieldTypes={customFieldTypes}
                    hiddenCustomFieldTypes={hiddenCustomFieldTypes}>
                    <TaskTableRow />
                </CollapsibleTask>
            );
        }

        return (
            <tr key={task.id}>
                <TaskTableRow />
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