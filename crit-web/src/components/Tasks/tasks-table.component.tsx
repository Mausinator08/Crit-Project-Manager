import { Table } from "react-bootstrap";
import { JSX, useContext, useEffect, useRef, useState } from "react";
import { Outlet, useParams } from "react-router-dom";

import { Task } from "../../models/task.model";
import { ThemeContext } from "../../contexts/Theme/theme-context";
import styles from "./tasks.module.scss";
import { CustomFieldType } from '../../models/custom-field-type.model';
import { Status } from "../../models/status.model";
import { ListTasks } from "../../functions/Tasks/list-tasks";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { AuthService } from "../../services/AuthService.service";
import { Priority } from "../../models/priority.model";
import { User } from "../../models/requests/user.model";

export interface TasksProps {
    tasks: Task[];
    customFieldTypes: CustomFieldType[];
    hiddenCustomFieldTypes: CustomFieldType[];
    users: User[];
    statuses: Status[];
    priorities: Priority[];
    selectedProjectId: string;
}

function listTaskListCustomColumn(fieldType: CustomFieldType): JSX.Element {
    return (
        <th>
            {fieldType.name}
        </th>
    );
}

function TaskTable(props: TasksProps): JSX.Element {
    const { selectedTaskId } = useParams();
    const { theme } = useContext(ThemeContext);
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const moduleContext = useRef(GetModuleContext('app'));
    const { getService } = useContext(moduleContext.current.context);
    const authService: AuthService = getService(AuthService);

    return (
        <div>
            <h2>Tasks</h2>
            <hr />
            <div className={styles.taskWrapper}>
                <Table responsive striped bordered hover variant={theme} className={selectedTaskId ? styles.taskListCondensed : styles.taskList}>
                    <thead>
                        <tr>
                            <th>
                                Title
                            </th>
                            <th>
                                Assigned User
                            </th>
                            <th>
                                Status
                            </th>
                            <th>
                                Priority
                            </th>
                            <th>
                                Complexity
                            </th>
                            <th>
                                Due Date
                            </th>
                            {props.customFieldTypes.length > 0 &&
                                props.customFieldTypes.filter(fieldType => !props.hiddenCustomFieldTypes.find(type => type.id === fieldType.id)).map<JSX.Element>(fieldType => listTaskListCustomColumn(fieldType))}
                        </tr>
                    </thead>
                    <tbody>
                        {props.tasks && props.tasks.map(task => task && ListTasks(task, props.users, props.statuses, props.priorities, props.customFieldTypes, props.hiddenCustomFieldTypes))}
                    </tbody>
                </Table>
                {selectedTaskId && (<Outlet />)}
            </div>
        </div >
    );
}

export default TaskTable;