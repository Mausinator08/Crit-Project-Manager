import { Table } from "react-bootstrap";
import { JSX, useContext, useEffect, useRef, useState } from "react";
import { Outlet, useParams } from "react-router-dom";

import { Task } from "../../models/task.model";
import { ThemeContext } from "../../contexts/Theme/theme-context";
import styles from "./tasks.module.scss";
import { CustomFieldType } from '../../models/custom-field-type.model';
import { GetLinks, GetLoggedOutLinks, Link } from "../../constants/nav-bar-links";
import { Status } from "../../models/status.model";
import { ListTasks } from "../../functions/Tasks/list-tasks";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { AuthService } from "../../services/AuthService.service";

export interface TasksProps {
    tasks: Task[];
    customFieldTypes: CustomFieldType[];
    hiddenCustomFieldTypeIds: string[];
    statuses: Status[];
    selectedProjectId: string;
}

function listTaskListCustomColumn(fieldType: CustomFieldType, hiddenCustomFieldTypeIds: string[]): JSX.Element {
    if (hiddenCustomFieldTypeIds.find(typeId => typeId === fieldType.id)) {
        return (
            <>
                {null}
            </>
        );
    }

    return (
        <th>
            {fieldType.name}
        </th>
    );
}

function Tasks(props: TasksProps): JSX.Element {
    const { selectedTaskId } = useParams();
    const { theme } = useContext(ThemeContext);
    const moduleContext = useRef(GetModuleContext('app'));
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
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
                            {props.customFieldTypes?.map<JSX.Element>(fieldType => listTaskListCustomColumn(fieldType, props.hiddenCustomFieldTypeIds))}
                        </tr>
                    </thead>
                    <tbody>
                        {props.tasks && props.tasks.map(task => task && ListTasks(task, props.statuses))}
                    </tbody>
                </Table>
                {selectedTaskId && (<Outlet />)}
            </div>
        </div >
    );
}

export default Tasks;