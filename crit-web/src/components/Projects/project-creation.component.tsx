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
                        return;
                    }

                    const newProject: ProjectRequest = {
                        name: newProjectName,
                        description: newProjectDescription,
                    };
                    projectService.CreateNewProject(newProject)
                        .then((createdProject) => {
                            setNewProjectName('');
                            setNewProjectDescription('');
                            let newProjectNameInput = (document.getElementById('newProjectName') as HTMLInputElement | null);
                            let newProjectDescriptionInput = (document.getElementById('newProjectDescripton') as HTMLInputElement | null);
                            if (newProjectNameInput) {
                                newProjectNameInput.value = '';
                            }
                            if (newProjectDescriptionInput) {
                                newProjectDescriptionInput.value = '';
                            }
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
                }} className={newProjectName ? 'project-action-icon-new' : 'project-action-icon-new-disabled'} />
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