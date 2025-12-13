import { JSX } from "react";
import { UseService } from "../../contexts/Module/module-context";
import { ProjectService } from "../../services/ProjectService.service";
import { Project } from "../../models/project.model";
import "./projects.scss";

export interface ProjectActionsProps {
	projects: Project[];
	setProjects: (projects: Project[]) => void;
	selectedProjects: string[];
	setSelectedProjects: (selectedProjects: string[]) => void;
	isLockedProjectsEnabled: boolean;
	setError: (error: string | null) => void;
}

function ProjectActions({
	projects,
	setProjects,
	selectedProjects,
	setSelectedProjects,
	isLockedProjectsEnabled,
	setError,
}: ProjectActionsProps): JSX.Element {
	const projectService: ProjectService = UseService('app', ProjectService);

	return (
		<div className="actions-panel">
			<button
				onClick={() => {
					projectService
						.GetAllProjects()
						.then((value) => {
							setProjects(value);
						})
						.catch((error: Error) => {
							console.error(error);
							setError(error.message);
							setProjects([]);
						});
				}}
			>
				Refresh
			</button>
			<button
				disabled={
					selectedProjects.length === 0 || isLockedProjectsEnabled
				}
				onClick={() => {
					if (isLockedProjectsEnabled) {
						alert(
							"Project deletions are locked. Please unlock them to delete projects."
						);
						return;
					}

					if (selectedProjects.length === 0) {
						alert("Please select at least one project to delete.");
						return;
					}

					for (let projectId in selectedProjects) {
						projectService
							.DeleteProject(selectedProjects[projectId])
							.then(() => {
								projectService
									.GetAllProjects()
									.then((value) => {
										setProjects(value);
									})
									.catch((error: Error) => {
										console.error(error);
										setError(error.message);
										setProjects([]);
									});
								selectedProjects.splice(
									selectedProjects.indexOf(
										selectedProjects[projectId]
									),
									1
								);
							})
							.catch((error: Error) => {
								console.error(error);
								setError(error.message);
							});
					}
				}}
			>
				{"Delete Project(s)"}
			</button>
			<button
				disabled={selectedProjects.length > 0}
				onClick={() => {
					setSelectedProjects(
						projects?.map((project) => project.id!) ?? []
					);
				}}
			>
				Select All Projects
			</button>
			<button
				disabled={selectedProjects.length === 0}
				onClick={() => {
					setSelectedProjects([]);
				}}
			>
				Deselect All Projects
			</button>
		</div>
	);
}

export default ProjectActions;
