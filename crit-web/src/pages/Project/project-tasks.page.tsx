import { useParams } from "react-router-dom";
import { GetEnvValues } from "../../constants/environment";
import { JSX, useEffect, useRef, useState } from "react";
import { Project } from '../../models/project.model';
import TaskTable from "../../components/Tasks/tasks-table.component";

async function getProject(projectId: string | undefined): Promise<Project | null> {
    return new Promise(async (resolve, reject): Promise<void> => {
        const response = await fetch(new URL(`${GetEnvValues()?.critApiUrl}/Project/${projectId}`), {
            method: 'GET',
            credentials: 'include',
            mode: 'cors',
        });

        if (response.status !== 200) {
            reject(await response.text());
            return;
        }

        const data: Project = await response.json() as Project;
        resolve(data);
    });
}

function ProjectTasks(): JSX.Element {
    const { projectId } = useParams();
    const [project, setProject] = useState<Project | null>(null);
    const [hasFetched, setHasFetched] = useState<boolean>(false);

    useEffect(() => {
        if (hasFetched) {
            return;
        }

        setHasFetched(true);

        getProject(projectId).then(value => {
            setProject(value);
        });
    }, [projectId]);

    return (
        <>
            <h2>{project?.name ?? '<no project name>'}</h2>
            <hr />
            {project && (<TaskTable selectedProjectId={project.id!} tasks={project?.tasks ?? []} customFieldTypes={project?.customFieldTypes ?? []} statuses={project?.statuses ?? []} hiddenCustomFieldTypeIds={project?.hiddenCustomFieldTypeIds} />)}
        </>
    );
}

export default ProjectTasks;