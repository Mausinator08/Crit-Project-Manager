import { JSX, useContext, useEffect, useRef, useState } from "react";
import { useParams } from "react-router-dom";
import { GetEnvValues } from "../../constants/environment";
import { Project } from "../../models/project.model";

import "./projects.scss";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { UserService } from "../../services/UserService.service";
import { ProjectService } from "../../services/ProjectService.service";
import { ApiResult } from "../../models/responses/api-result.model";
import { Dropdown } from "react-bootstrap";

function ProjectOptions(): JSX.Element {
    const { projectId } = useParams();
    const [project, setProject] = useState<Project | null>(null);
    const [hasFetched, setHasFetched] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const moduleContext = useRef(GetModuleContext("app"));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService("ProjectService");
    const userService: UserService = getService("UserService");
    const [userId, setUserId] = useState<string | null>(null);

    useEffect(() => {
        if (hasFetched) {
            return;
        }

        setHasFetched(true);

        if (!projectId) {
            return;
        }

        projectService.GetProject(projectId)
            .then(value => {
                setProject(value);
            })
            .catch((error: Error) => {
                console.error(error);
                setProject(null);
                setError(error.message);
            });

        userService.GetLoggedInUserId()
            .then((result) => {
                if (result) {
                    if (!result.data) {
                        console.error("No user ID found in the result.");
                        setError("No user ID found in the result.");
                        setUserId(null);
                        return;
                    }

                    setUserId(result.data);
                }
            })
            .catch((error: ApiResult<string | null>) => {
                console.error(error.message);
                (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                    console.error(err);
                });
                setError(error.message);
            });
    }, [projectId]);

    return (
        <>
            <h2>{project?.name ?? '<no project name>'}</h2>
            <h3>Options</h3>
            <hr />
            {error && <p style={{ color: "red" }}>{error}</p>}
            {project && (
                <div>
                    <div>
                        <label htmlFor="project-name">Project Name</label>
                        <input type="text" id="project-name" value={project.name} />
                    </div>
                    <div>
                        <label htmlFor="project-description">Project Description</label>
                        <textarea id="project-description" value={project.description} />
                    </div>
                    <div>
                    </div>
                </div>
            )}
        </>
    );
}

export default ProjectOptions;