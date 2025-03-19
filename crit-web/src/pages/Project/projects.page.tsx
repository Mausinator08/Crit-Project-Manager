import { JSX, useContext, useEffect, useRef, useState } from "react";
import { Outlet, useParams } from "react-router-dom";

import { Project } from "../../models/project.model";
import { ProjectService } from "../../services/ProjectService.service";
import { GetModuleContext } from "../../contexts/Module/module-context";
import "./projects.scss";
import ProjectsTable from "../../components/Projects/projects-table.component";
import ProjectSettings from "../../components/Projects/project-settings.component";
import ProjectActions from "../../components/Projects/project-actions.component";
import ProjectCreation from "../../components/Projects/project-creation.component";

function Projects(): JSX.Element {
    const { projectId } = useParams();
    const moduleContext = useRef(GetModuleContext('projects'));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService(ProjectService);

    const [projects, setProjects] = useState<Project[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [isAutoRefreshEnabled, setIsAutoRefreshEnabled] = useState<boolean>(localStorage.getItem('isAutoRefreshEnabled') === 'true');
    const [autoRefreshInterval, setAutoRefreshInterval] = useState<number>(parseInt(localStorage.getItem('autoRefreshInterval') || '1', 10));
    const [autoRefreshIntervalInstance, setAutoRefreshIntervalInstance] = useState<NodeJS.Timeout | null>(null);
    const [selectedProjects, setSelectedProjects] = useState<string[]>([]);
    const [isLockedProjectsEnabled, setIsLockedProjectsEnabled] = useState<boolean>(localStorage.getItem('isLockedProjectsEnabled') === 'true');

    useEffect(() => {
        projectService.GetAllProjects()
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
            projectService.GetAllProjects()
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
            <ProjectSettings isAutoRefreshEnabled={isAutoRefreshEnabled}
                setIsAutoRefreshEnabled={setIsAutoRefreshEnabled}
                autoRefreshInterval={autoRefreshInterval}
                setAutoRefreshInterval={setAutoRefreshInterval}
                isLockedProjectsEnabled={isLockedProjectsEnabled}
                setIsLockedProjectsEnabled={setIsLockedProjectsEnabled}
            />
            <hr />
            <ProjectActions projects={projects}
                setProjects={setProjects}
                selectedProjects={selectedProjects}
                setSelectedProjects={setSelectedProjects}
                isLockedProjectsEnabled={isLockedProjectsEnabled}
                setError={setError} />
            <hr />
            <div className="projects-panel">
                {error && <p style={{ color: "red" }}>{error}</p>}
                {projectId ? <Outlet /> :
                    projects && <ProjectsTable
                        projects={projects}
                        setProjects={setProjects}
                        selectedProjects={selectedProjects}
                        setSelectedProjects={setSelectedProjects}
                        isLockedProjectsEnabled={isLockedProjectsEnabled}
                        setError={setError} />
                }
                <hr />
                <ProjectCreation
                    setProjects={setProjects}
                    setError={setError} />
            </div>
        </div >
    );
}

export default Projects;