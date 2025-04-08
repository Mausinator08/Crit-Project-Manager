import { JSX, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { GetEnvValues } from "../../constants/environment";
import { Project } from "../../models/project.model";

import "./projects.scss";

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

function ProjectOptions(): JSX.Element {
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
            <h3>Options</h3>
            <hr />
            {project && (
                <div className="project-options">
                    <div className="project-option">
                        <label htmlFor="project-name">Project Name</label>
                        <input type="text" id="project-name" value={project.name} readOnly />
                    </div>
                    <div className="project-option">
                        <label htmlFor="project-description">Project Description</label>
                        <textarea id="project-description" value={project.description} readOnly />
                    </div>
                </div>
            )}
        </>
    );
}

export default ProjectOptions;