import { JSX, useContext, useRef, useState } from "react";
import { Project } from "../../models/project.model";
import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMinus, faPencil } from "@fortawesome/free-solid-svg-icons";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { ProjectService } from "../../services/ProjectService.service";
import './projects.scss';

export interface ProjectTableProps {
    projects: Project[];
    setProjects: (projects: Project[]) => void;
    selectedProjects: string[];
    setSelectedProjects: (selectedProjects: string[]) => void;
    isLockedProjectsEnabled: boolean;
    setError: (error: string) => void;
}

function ProjectsTable({ projects,
    setProjects,
    selectedProjects,
    setSelectedProjects,
    isLockedProjectsEnabled,
    setError }: ProjectTableProps): JSX.Element {
    const moduleContext = useRef(GetModuleContext('projects'));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService(ProjectService);
    const [editingProjectName, setEditingProjectName] = useState<string | null>(null);
    const [editingProjectDescription, setEditingProjectDescription] = useState<string | null>(null);

    return (
        <table width="100%">
            <thead>
                <tr>
                    <th>Select</th>
                    <th>Edit</th>
                    <th>Project Name</th>
                    <th>Project Description</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                {projects?.length > 0 ? projects?.map<JSX.Element>(project => {
                    return (
                        <tr key={`project_row_${project.id}`}>
                            <td>
                                <input type="checkbox" name="project_select" onChange={(e) => {
                                    if (selectedProjects.includes(project.id!)) {
                                        if (!e.target.checked) {
                                            setSelectedProjects(selectedProjects.filter(id => id !== project.id));
                                        }
                                    } else {
                                        if (e.target.checked) {
                                            setSelectedProjects([...selectedProjects, project.id!]);
                                        }
                                    }
                                }} checked={((): boolean => {
                                    return selectedProjects.includes(project.id!);
                                })()} />
                            </td>
                            <td>
                                <NavLink to={`/Projects/${project.id}`} key={`/Projects/${project.id!}`}>
                                    <FontAwesomeIcon icon={faPencil} />
                                </NavLink>
                            </td>
                            <td>
                                <h5 onClick={() => setEditingProjectName(project.id!)}>
                                    {editingProjectName === project.id ? (<input
                                        type="text"
                                        id={`project_name_for_${project.id}`}
                                        defaultValue={project.name}
                                        onBlur={(e) => {
                                            const proj: Project | undefined = projects.find(p => p.id === e.target.id.replace(/project_name_for_/g, ''));

                                            if (!proj) {
                                                console.error(`Project with id ${e.target.id.replace(/project_name_for_/g, '')} not found.`);
                                                return;
                                            }

                                            proj.name = e.target.value;
                                            projectService.UpdateProject(proj)
                                                .then(() => {
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
                                            setEditingProjectName(null);
                                        }}
                                    />) : project.name}
                                </h5>
                            </td>
                            <td>
                                <h5 onClick={() => setEditingProjectDescription(project.description ?? null)}>{editingProjectDescription === project.description ? (<input
                                    type="text"
                                    id={`project_description_for_${project.id}`}
                                    defaultValue={project.description}
                                    onBlur={(e) => {
                                        const proj: Project | undefined = projects.find(p => p.id === e.target.id.replace(/project_description_for_/g, ''));

                                        if (!proj) {
                                            console.error(`Project with id ${e.target.id.replace(/project_description_for_/g, '')} not found.`);
                                            return;
                                        }

                                        project.description = e.target.value;
                                        projectService.UpdateProject(project)
                                            .then(() => {
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
                                        setEditingProjectDescription(null);
                                    }}
                                />) : project.description}
                                </h5>
                            </td>
                            <td>
                                <FontAwesomeIcon icon={faMinus} className={isLockedProjectsEnabled ? 'project-action-icon-disabled' : 'project-action-icon'} onClick={() => {
                                    if (isLockedProjectsEnabled) {
                                        return;
                                    }

                                    projectService.DeleteProject(project.id!)
                                        .then(() => {
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
                                }} />
                            </td>
                        </tr>
                    );
                }) : (
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <p>No projects found. Please create a new project.</p>
                        </td>
                        <td>
                        </td>
                    </tr>
                )}
            </tbody>
        </table>
    );
}

export default ProjectsTable;