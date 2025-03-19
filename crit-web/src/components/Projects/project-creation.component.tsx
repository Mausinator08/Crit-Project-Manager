import { JSX, useContext, useRef, useState } from "react";
import { ProjectRequest } from "../../models/requests/project-request.model";
import { Project } from "../../models/project.model";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { ProjectService } from "../../services/ProjectService.service";
import './projects.scss';

export interface ProjectCreationProps {
    setProjects: (projects: Project[]) => void;
    setError: (error: string | null) => void;
}

function ProjectCreation({
    setProjects,
    setError,
}: ProjectCreationProps): JSX.Element {
    const moduleContext = useRef(GetModuleContext('projects'));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService(ProjectService);
    const [newProjectName, setNewProjectName] = useState<string>("");
    const [newProjectDescription, setNewProjectDescription] = useState<string>("");

    return (
        <>
            <h3>Create New Project</h3>
            <div className="project-row" key={`project_row_new_project`}>
                <FontAwesomeIcon icon={faPlus} onClick={() => {
                    if (newProjectName === "") {
                        setError('Please enter a name for the new project.');
                        alert('Please enter a name for the new project.');
                        return;
                    }

                    const newProject: ProjectRequest = {
                        name: newProjectName,
                        description: newProjectDescription,
                    };
                    projectService.CreateNewProject(newProject)
                        .then((createdProject) => {
                            projectService.GetAllProjects()
                                .then(value => {
                                    setProjects(value);
                                })
                                .catch((error: Error) => {
                                    console.error(error);
                                    setError(error.message);
                                    setProjects([]);
                                });
                        })
                        .catch((error: Error) => {
                            console.error(error);
                            setError(error.message);
                        });
                }} className="clickable project-action-icon" />
                <input type="text" id="newProjectName" onBlur={(e) => {
                    setNewProjectName(e.target.value);
                }} />
                <input type="text" id="newProjectDescription" onBlur={(e) => {
                    setNewProjectDescription(e.target.value);
                }} />
            </div>
        </>
    );
}

export default ProjectCreation;