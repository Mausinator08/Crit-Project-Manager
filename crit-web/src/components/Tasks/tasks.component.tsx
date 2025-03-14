import { Table } from "react-bootstrap";
import { JSX, useContext, useState } from "react";
import { Outlet, useParams } from "react-router-dom";

import { Task } from "../../models/task.model";
import { ThemeContext } from "../../contexts/Theme/theme-context";
import styles from "./tasks.module.scss";
import { CustomFieldType } from '../../models/custom-field-type.model';
import { GetLinks, Link } from "../../constants/nav-bar-links";
import { Status } from "../../models/status.model";
import { CreateTasks } from "../../functions/Tasks/create-tasks";

export interface TasksProps {
    tasks: Task[];
    customFieldTypes: CustomFieldType[];
    hiddenCustomFieldTypeIds: string[];
    statuses: Status[];
    selectedProjectId: string;
}

function createTaskListCustomColumn(fieldType: CustomFieldType, hiddenCustomFieldTypeIds: string[]): JSX.Element {
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

function createTaskRow(task: Task, statuses: Status[], links: Link[]): JSX.Element | undefined {
    const link: Link | undefined = links.find(link => (link.data as Task | undefined)?.id === task.id);

    if (link) {
        return CreateTasks(link, task, statuses);
    }

    return (
        <>
            {null}
        </>
    );
}

function Tasks(props: TasksProps): JSX.Element {
    const { selectedTaskId } = useParams();
    const { theme } = useContext(ThemeContext);
    const [links, setLinks] = useState<Link[]>([]);

    GetLinks().then((links: Link[]) => setLinks(links));

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
                            {props.customFieldTypes?.map<JSX.Element>(fieldType => createTaskListCustomColumn(fieldType, props.hiddenCustomFieldTypeIds))}
                        </tr>
                    </thead>
                    <tbody>
                        {props.tasks.map<JSX.Element | undefined>(task => createTaskRow(task, props.statuses, links))}
                    </tbody>
                </Table>
                {selectedTaskId && (<Outlet />)}
            </div>
        </div >
    );
}

export default Tasks;