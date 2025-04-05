import { JSX, useContext, useEffect, useRef, useState } from "react";
import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { Task } from "../../models/task.model";
import { Status } from "../../models/status.model";
import CollapsibleTask from "../../components/CollapsibleTask/collapsible-task.component";
import UserSelect from "../../components/UserSelect/user-select.component";
import StatusSelect from "../../components/StatusSelect/status-select.component";
import { faClipboardCheck } from "@fortawesome/free-solid-svg-icons";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { UserService } from "../../services/UserService.service";
import { Priority } from "../../models/priority.model";
import { User } from "../../models/user.model";
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
    hiddenCustomFieldTypeIds: string[],
    isSubtask?: boolean,
): JSX.Element {
    const moduleContext = useRef(GetModuleContext('app'));
    const { getService } = useContext(moduleContext.current.context);
    const userService: UserService = getService(UserService);
    const [assignedUser, setAssignedUser] = useState<string | undefined>(undefined);
    const [status, setStatus] = useState<string | undefined>(undefined)
    const [priority, setPriority] = useState<string | undefined>(undefined);
    const [complexity, setComplexity] = useState<number | undefined>(undefined);
    const [dueDate, setDueDate] = useState<Date | undefined>(undefined);
    const [customFields, setCustomFields] = useState<(string | undefined)[]>([]);

    function onAssignedUserSelect(value: string | undefined | null): void {
        if (value) {
            setAssignedUser(value);
            const user = users.find(u => u.id === value);
            if (user) {
                task.assignedUserId = user.id;
            }
        } else {
            setAssignedUser(undefined);
            task.assignedUserId = undefined;
        }
    }

    function onStatusSelect(value: string | undefined | null): void {
        if (value) {
            setStatus(value);
            const status = statuses.find(s => s.id === value);
            if (status) {
                task.statusId = status.id;
            }
        } else {
            setStatus(undefined);
            task.statusId = undefined;
        }
    }

    function onPrioritySelect(value: string | undefined | null): void {
        if (value) {
            setPriority(value);
            const priority = priorities.find(s => s.id === value);
            if (priority) {
                task.priorityId = priority.id;
            }
        } else {
            setPriority(undefined);
            task.priorityId = undefined;
        }
    }

    function onComplexitySelect(value: number | undefined | null): void {
        if (value) {
            setComplexity(value);
            task.complexity = value;
        } else {
            setComplexity(undefined);
            task.complexity = undefined;
        }
    }

    function onDueDateSelect(value: Date | undefined | null): void {
        if (value) {
            setDueDate(value);
            task.dueDate = value;
        }
        else {
            setDueDate(undefined);
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
            setCustomFields(task.customFields.map(cf => cf.id));
        } else {
            task.customFields = task.customFields.map(cf => {
                if (cf.id === value) {
                    cf.value = undefined;
                }
                return cf;
            });
            setCustomFields(task.customFields.map(cf => cf.id));
        }
    }

    useEffect(() => {
        if (task.assignedUserId) {
            const user = users.find(u => u.id === task.assignedUserId);
            setAssignedUser(user?.id);
        } else {
            setAssignedUser(undefined);
        }

        if (task.statusId) {
            const status = statuses.find(s => s.id === task.statusId);
            setStatus(status?.id);
        } else {
            setStatus(undefined);
        }

        if (task.priorityId) {
            const priority = priorities.find(s => s.id === task.priorityId);
            setStatus(priority?.id);
        } else {
            setStatus(undefined);
        }

        if (task.complexity) {
            setComplexity(task.complexity);
        } else {
            setComplexity(undefined);
        }

        if (task.dueDate) {
            setDueDate(task.dueDate);
        } else {
            setDueDate(undefined);
        }

        setCustomFields(task.customFields.map(cf => cf.id));
    }, []);

    function TaskTableRow(): JSX.Element {
        return (
            <>
                <td>
                    {task.title}
                </td>
                <td>
                    <UserSelect users={users} assignedUser={assignedUser} onAssignedUserSelect={onAssignedUserSelect} taskId={task.id!} />
                </td>
                <td>
                    <StatusSelect statuses={statuses} onStatusSelect={onStatusSelect} taskId={task.id!} status={status} />
                </td>
                <td>
                    <PrioritySelect priorities={priorities} onPrioritySelect={onPrioritySelect} taskId={task.id!} priority={priority} />
                </td>
                <td>
                    <ComplexitySelect taskId={task.id!} onComplexitySelect={onComplexitySelect} complexity={complexity} />
                </td>
                <td>
                    <DueDateSelect taskId={task.id!} dueDate={dueDate} onDueDateSelect={onDueDateSelect} />
                </td>
                <td>
                    {customFieldTypes.length > 0 &&
                        customFieldTypes.filter(fieldType => !hiddenCustomFieldTypeIds.find(typeId => typeId === fieldType.id)).map<JSX.Element>(fieldType =>
                            <>
                                <CustomFieldSelect taskId={task.id!} customFields={task.customFields} customFieldType={fieldType} onCustomFieldSelect={onCustomFieldSelect} customField={task.customFields.find(cf => cf.customFieldTypeId === fieldType.id)?.id} />
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
                <CollapsibleTask key={task.id} task={task} users={users} statuses={statuses} priorities={priorities} isSubtask={isSubtask}>
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