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
	const moduleContext = useRef(GetModuleContext('app'));
	const { getService } = useContext(moduleContext.current.context);
	const projectService: ProjectService = getService(ProjectService);
	const [projectsStates, setProjectStates] = useState<{
		projects: Project[];
		error: string | null;
		isAutoRefreshEnabled: boolean;
		autoRefreshInterval: number;
		autoRefreshIntervalInstance: NodeJS.Timeout | null;
		selectedProjects: string[];
		isLockedProjectsEnabled: boolean;
		hasFetched: boolean;
	}>({
		projects: [],
		error: null,
		isAutoRefreshEnabled: localStorage.getItem("isAutoRefreshEnabled") === "true",
		autoRefreshInterval: parseInt(localStorage.getItem("autoRefreshInterval") || "1", 10),
		autoRefreshIntervalInstance: null,
		selectedProjects: [],
		isLockedProjectsEnabled: localStorage.getItem("isLockedProjectsEnabled") === "true",
		hasFetched: false,
	});

	useEffect(() => {
		if (projectsStates.hasFetched) {
			return;
		}

		setProjectStates(prevState => ({ ...prevState, hasFetched: true }));

		projectService
			.GetAllProjects()
			.then((value) => {
				setProjectStates(prevState => ({ ...prevState, projects: value }));
			})
			.catch((error: Error) => {
				console.error(error);
				setProjectStates(prevState => ({ ...prevState, error: error.message, projects: [] }));
			});

		setProjectStates(prevState => ({ ...prevState, isAutoRefreshEnabled: localStorage.getItem("isAutoRefreshEnabled") === "true" }));
		let refreshInterval: number = parseInt(
			localStorage.getItem("autoRefreshInterval") || "1",
			10
		);

		if (refreshInterval < 1 || refreshInterval > 60) {
			refreshInterval = 1;
		}

		setProjectStates(prevState => ({ ...prevState, autoRefreshInterval: refreshInterval }));
	}, []);

	useEffect(() => {
		localStorage.setItem(
			"isAutoRefreshEnabled",
			projectsStates.isAutoRefreshEnabled.toString()
		);
		localStorage.setItem(
			"autoRefreshInterval",
			projectsStates.autoRefreshInterval.toString()
		);

		if (projectsStates.autoRefreshIntervalInstance) {
			clearInterval(projectsStates.autoRefreshIntervalInstance);
		}

		if (!projectsStates.isAutoRefreshEnabled) return;

		setProjectStates(prevState => ({
			...prevState, autoRefreshIntervalInstance: setInterval(() => {
				projectService
					.GetAllProjects()
					.then((value) => {
						setProjectStates(prevState => ({ ...prevState, projects: value }));
					})
					.catch((error: Error) => {
						console.error(error);
						setProjectStates(prevState => ({ ...prevState, error: error.message, projects: [] }));
					});
			}, projectsStates.autoRefreshInterval * 60000)
		}));
		// Convert minutes to milliseconds
	}, [
		projectsStates.isAutoRefreshEnabled,
		projectsStates.autoRefreshInterval,
	]);

	return (
		<div>
			<h2>
				{projectId
					? projectsStates.projects.find((p) => p.id === projectId)?.name
					: "Projects"}
			</h2>
			<hr />
			<ProjectSettings
				isAutoRefreshEnabled={projectsStates.isAutoRefreshEnabled}
				setIsAutoRefreshEnabled={(value) => setProjectStates(prevState => ({ ...prevState, isAutoRefreshEnabled: value }))}
				autoRefreshInterval={projectsStates.autoRefreshInterval}
				setAutoRefreshInterval={(value) => setProjectStates(prevState => ({ ...prevState, autoRefreshInterval: value }))}
				isLockedProjectsEnabled={projectsStates.isLockedProjectsEnabled}
				setIsLockedProjectsEnabled={(value) => setProjectStates(prevState => ({ ...prevState, isLockedProjectsEnabled: value }))}
			/>
			<hr />
			<ProjectActions
				projects={projectsStates.projects}
				setProjects={(value) => setProjectStates(prevState => ({ ...prevState, projects: value }))}
				selectedProjects={projectsStates.selectedProjects}
				setSelectedProjects={(value) => setProjectStates(prevState => ({ ...prevState, selectedProjects: value }))}
				isLockedProjectsEnabled={projectsStates.isLockedProjectsEnabled}
				setError={(value) => setProjectStates(prevState => ({ ...prevState, error: value }))}
			/>
			<hr />
			<div className="projects-panel">
				{projectsStates.error && <p style={{ color: "red" }}>{projectsStates.error}</p>}
				{projectId ? (
					<Outlet />
				) : (
					projectsStates.projects && (
						<ProjectsTable
							projects={projectsStates.projects}
							setProjects={(value) => setProjectStates(prevState => ({ ...prevState, projects: value }))}
							selectedProjects={projectsStates.selectedProjects}
							setSelectedProjects={(value) => setProjectStates(prevState => ({ ...prevState, selectedProjects: value }))}
							isLockedProjectsEnabled={projectsStates.isLockedProjectsEnabled}
							setError={(value) => setProjectStates(prevState => ({ ...prevState, error: value }))}
						/>
					)
				)}
				<hr />
				{projectId ? (
					"Create a new task placeholder"
				) : (
					<ProjectCreation
						setProjects={(value) => setProjectStates(prevState => ({ ...prevState, projects: value }))}
						setError={(value) => setProjectStates(prevState => ({ ...prevState, error: value }))}
					/>
				)}
			</div>
		</div>
	);
}

export default Projects;
