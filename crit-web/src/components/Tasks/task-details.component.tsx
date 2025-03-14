
import { useParams } from "react-router-dom";
import { JSX, useEffect, useState } from "react";

import { GetEnvValues } from "../../constants/environment";
import { Task } from "../../models/task.model";

async function getTask(taskId: string | undefined): Promise<Task | null> {
    return new Promise(async (resolve, reject): Promise<void> => {
        const response = await fetch(new URL(`${GetEnvValues()?.critApiUrl}/Project/${taskId}`), {
            method: 'GET',
            mode: 'cors',
            credentials: 'include',
        });

        if (response.status !== 200) {
            reject(await response.text());
            return;
        }

        const data: Task = await response.json() as Task;
        resolve(data);
    });
}

function TaskDetails(): JSX.Element {
    const { taskId } = useParams();
    const [task, setTask] = useState<Task | null>(null);
    useEffect(() => {
        getTask(taskId).then(value => {
            setTask(value);
        });
    }, [taskId]);

    return (
        <div>
            <h2>Task Details for: {task?.title ?? '<new task>'}</h2>
            <hr />
        </div>
    );
}

export default TaskDetails;