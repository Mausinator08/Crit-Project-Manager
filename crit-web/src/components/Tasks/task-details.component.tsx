
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
    const [taskDetailsStates, setTaskDetailsStates] = useState<{
        task: Task | null;
        hasFetched: boolean;
    }>({
        task: null,
        hasFetched: false,
    });

    useEffect(() => {
        if (taskDetailsStates.hasFetched) {
            return;
        }

        setTaskDetailsStates((prevState) => ({ ...prevState, hasFetched: true }));

        getTask(taskId).then(value => {
            setTaskDetailsStates((prevState) => ({ ...prevState, task: value }));
        });
    }, [
        taskId,
    ]);

    return (
        <div>
            <h2>Task Details for: {taskDetailsStates.task?.title ?? '<new task>'}</h2>
            <hr />
        </div>
    );
}

export default TaskDetails;