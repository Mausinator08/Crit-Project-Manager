import { JSX, useContext, useEffect, useRef, useState } from "react";
import { NavLink, Outlet, useParams } from "react-router-dom";

import { Project } from "../../models/project.model";
import { ProjectService } from "../../services/ProjectService.service";
import { GetModuleContext } from "../../contexts/Module/module-context";
import "./projects.scss";
import { Tooltip } from "react-tooltip";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMinus, faPencil, faPlus } from "@fortawesome/free-solid-svg-icons";
import { ProjectRequest } from "../../models/requests/project-request.model";

async function getProjects(projectService: ProjectService): Promise<Project[] | null> {
    return new Promise((resolve, reject): void => {
        projectService.GetAllProjects()
            .then((data: Project[]) => {
                resolve(data);
            })
            .catch((error: Error) => {
                reject(error);
            });
    });
}

function Projects(): JSX.Element {
    const { projectId } = useParams();
    const moduleContext = useRef(GetModuleContext('projects'));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService(ProjectService);

    const [projects, setProjects] = useState<Project[] | null>([]);
    const [error, setError] = useState<string | null>(null);
    const [isAutoRefreshEnabled, setIsAutoRefreshEnabled] = useState<boolean>(localStorage.getItem('isAutoRefreshEnabled') === 'true');
    const [autoRefreshInterval, setAutoRefreshInterval] = useState<number>(parseInt(localStorage.getItem('autoRefreshInterval') || '1', 10));
    const [autoRefreshIntervalInstance, setAutoRefreshIntervalInstance] = useState<NodeJS.Timeout | null>(null);
    const [selectedProjects, setSelectedProjects] = useState<string[]>([]);
    const [isLockedProjectsEnabled, setIsLockedProjectsEnabled] = useState<boolean>(localStorage.getItem('isLockedProjectsEnabled') === 'true');
    const [editingProjectName, setEditingProjectName] = useState<string | null>(null);
    const [editingProjectDescription, setEditingProjectDescription] = useState<string | null>(null);
    const [newProjectName, setNewProjectName] = useState<string>("");
    const [newProjectDescription, setNewProjectDescription] = useState<string>("");

    useEffect(() => {
        getProjects(projectService)
            .then(value => {
                setProjects(value);
            })
            .catch((error: Error) => {
                console.error(error);
                setError(error.message);
                setProjects([]);
            });

        setIsAutoRefreshEnabled(localStorage.getItem('isAutoRefreshEnabled') === 'true');
        let refreshInterval: number = parseInt(localStorage.getItem('autoRefreshInterval') || '1', 10);

        if (refreshInterval < 1 || refreshInterval > 60) {
            refreshInterval = 1;
        }

        setAutoRefreshInterval(refreshInterval);
    }, []);

    useEffect(() => {
        localStorage.setItem('isAutoRefreshEnabled', isAutoRefreshEnabled.toString());
        localStorage.setItem('autoRefreshInterval', autoRefreshInterval.toString());

        if (autoRefreshIntervalInstance) {
            clearInterval(autoRefreshIntervalInstance);
        }

        if (!isAutoRefreshEnabled) return;

        setAutoRefreshIntervalInstance(setInterval(() => {
            getProjects(projectService)
                .then(value => {
                    setProjects(value);
                })
                .catch((error: Error) => {
                    console.error(error);
                    setError(error.message);
                    setProjects([]);
                });
        }, autoRefreshInterval * 60000)); // Convert minutes to milliseconds
    }, [isAutoRefreshEnabled, autoRefreshInterval]);

    return (
        <div>
            <h2>Projects</h2>
            <hr />
            <div className="settings-panel">
                <label>Settings</label>
                <hr />
                <div className="settings">
                    <div className="settings-item">
                        <label htmlFor="autoRefresh" className="checkbox-label">
                            <input
                                id="autoRefresh"
                                type="checkbox"
                                checked={isAutoRefreshEnabled}
                                onChange={(e) => setIsAutoRefreshEnabled(e.target.checked)}
                            /><span className="checkbox-span">Auto Refresh</span>
                        </label>
                        <br />
                        {isAutoRefreshEnabled && (
                            <label htmlFor="interval">
                                {'Interval (minutes):'}
                                <input
                                    id="interval"
                                    type="number"
                                    value={autoRefreshInterval}
                                    onChange={(e) => {
                                        if (!isNaN(e.target.valueAsNumber) && e.target.valueAsNumber >= 1 && e.target.valueAsNumber <= 60) {
                                            setAutoRefreshInterval(e.target.valueAsNumber);
                                        } else {
                                            e.target.valueAsNumber = 1;
                                            setAutoRefreshInterval(1);
                                        }
                                    }}
                                    min={1}
                                    max={60}
                                    minLength={1}
                                    maxLength={2}
                                />
                            </label>
                        )}
                    </div>
                    <div className="settings-item">
                        <label htmlFor="lockProjects" className="checkbox-label">
                            <input
                                id="lockProjects"
                                type="checkbox"
                                data-tooltip-id="lockProjectsTooltip"
                                data-tooltip-content="Locking projects will prevent them from accidentally being deleted or renamed. (Clicking on a project and making changes to project options or tasks will still be allowed.)"
                                checked={isLockedProjectsEnabled}
                                onChange={(e) => {
                                    setIsLockedProjectsEnabled(e.target.checked);
                                    localStorage.setItem('isLockProjectDeletionsEnabled', e.target.checked.toString());
                                }} /><Tooltip id="lockProjectsTooltip" /><span className="checkbox-span">Lock Projects</span>
                        </label>
                    </div>
                </div>
            </div >
            <hr />
            <div className="actions-panel">
                <button onClick={() => {
                    getProjects(projectService)
                        .then(value => {
                            setProjects(value);
                        })
                        .catch((error: Error) => {
                            console.error(error);
                            setError(error.message);
                            setProjects([]);
                        });
                }}>Refresh</button>
                <button disabled={selectedProjects.length === 0 || isLockedProjectsEnabled} onClick={() => {
                    if (isLockedProjectsEnabled) {
                        alert('Project deletions are locked. Please unlock them to delete projects.');
                        return;
                    }

                    if (selectedProjects.length === 0) {
                        alert('Please select at least one project to delete.');
                        return;
                    }

                    for (let projectId in selectedProjects) {
                        projectService.DeleteProject(selectedProjects[projectId])
                            .then(() => {
                                getProjects(projectService)
                                    .then(value => {
                                        setProjects(value);
                                    })
                                    .catch((error: Error) => {
                                        console.error(error);
                                        setError(error.message);
                                        setProjects([]);
                                    });
                                selectedProjects.splice(selectedProjects.indexOf(selectedProjects[projectId]), 1);
                            })
                            .catch((error: Error) => {
                                console.error(error);
                                setError(error.message);
                            });
                    }
                }}>{'Delete Project(s)'}</button>
                <button disabled={selectedProjects.length > 0} onClick={() => {
                    setSelectedProjects(projects?.map(project => project.id) ?? []);
                }}>Select All Projects</button>
                <button disabled={selectedProjects.length === 0} onClick={() => {
                    setSelectedProjects([]);
                }}>Deselect All Projects</button>
            </div>
            <hr />
            <div className="projects-panel">
                {error && <p style={{ color: "red" }}>{error}</p>}
                {projectId ? <Outlet /> :
                    projects &&
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
                                                if (selectedProjects.includes(project.id)) {
                                                    if (!e.target.checked) {
                                                        setSelectedProjects(selectedProjects.filter(id => id !== project.id));
                                                    }
                                                } else {
                                                    if (e.target.checked) {
                                                        setSelectedProjects([...selectedProjects, project.id]);
                                                    }
                                                }
                                            }} checked={((): boolean => {
                                                return selectedProjects.includes(project.id);
                                            })()} />
                                        </td>
                                        <td>
                                            <NavLink to={`/Projects/${project.id}`} key={`/Projects/${project.id}`}>
                                                <FontAwesomeIcon icon={faPencil} />
                                            </NavLink>
                                        </td>
                                        <td>
                                            <h5 onClick={() => setEditingProjectName(project.id)}>
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
                                                                getProjects(projectService)
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
                                                            getProjects(projectService)
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
                                            <FontAwesomeIcon icon={faMinus} className={isLockedProjectsEnabled ? 'disabled project-action-icon' : 'project-action-icon'} onClick={() => {
                                                if (isLockedProjectsEnabled) {
                                                    alert('Project deletions are locked. Please unlock them to delete projects.');
                                                    return;
                                                }

                                                projectService.DeleteProject(project.id)
                                                    .then(() => {
                                                        getProjects(projectService)
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
                }
                <hr />
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
                                getProjects(projectService)
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
            </div>
        </div >
    );
}

export default Projects;