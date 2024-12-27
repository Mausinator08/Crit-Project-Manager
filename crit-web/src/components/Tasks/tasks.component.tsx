import { Table } from "react-bootstrap";
import { useContext, useState } from "react";
import { Outlet, useParams } from "react-router-dom";

import { Task } from "../../models/task.model";
import { ThemeContext } from "../../contexts/theme-context";
import styles from "./tasks.module.scss";
import TaskDetails from "../TaskDetails/task-details.component";
import { CustomField } from "../../models/custom-field.model";
import { CustomFieldType } from '../../models/custom-field-type.model';
import { Link, links } from "../../constants/nav-bar-links";
import { CreateLinks } from '../CollapsibleTaskItem/collapsible-task-item.component';
import { Status } from "../../models/status.model";

export interface TasksProps {
    tasks: Task[];
    customFieldTypes: CustomFieldType[];
    statuses: Status[];
    selectedProjectId: string;
}

function createTaskListCustomColumn(fieldType: CustomFieldType): JSX.Element {
    return (
        <th>
            {fieldType.name}
        </th>
    );
}

function createTaskListItem(task: Task, statuses: Status[]): JSX.Element | undefined {
    const link: Link | undefined = links.find(link => (link.data as Task | undefined)?.id === task.id);

    if (link) {
        return (
            <tr>
                {CreateLinks(link, task, statuses)}
            </tr>
        );
    }

    return undefined;
}

function Tasks(props: TasksProps): JSX.Element {
    const { selectedTaskId } = useParams();
    const { theme } = useContext(ThemeContext);

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
                            {props.customFieldTypes?.map<JSX.Element>(createTaskListCustomColumn)}
                        </tr>
                    </thead>
                    <tbody>
                        {props.tasks.map<JSX.Element | undefined>(task => createTaskListItem(task, props.statuses))}
                    </tbody>
                </Table>
                {selectedTaskId && (<Outlet />)}
            </div>
        </div >
    );
}

export default Tasks;