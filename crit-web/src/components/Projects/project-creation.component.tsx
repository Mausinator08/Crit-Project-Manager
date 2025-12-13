import { JSX, useState } from "react";
import { ProjectRequest } from "../../models/requests/project-request.model";
import { Project } from "../../models/project.model";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { UseService } from "../../contexts/Module/module-context";
import { ProjectService } from "../../services/ProjectService.service";

import "./projects.scss";

export interface ProjectCreationProps {
	setProjects: (projects: Project[]) => void;
	setError: (error: string | null) => void;
}

function ProjectCreation({
	setProjects,
	setError,
}: ProjectCreationProps): JSX.Element {
	const projectService: ProjectService = UseService('app', ProjectService);
	const [projectCreationStates, setProjectCreationStates] = useState<{
		newProjectName: string;
		newProjectDescription: string;
	}>({
		newProjectName: "",
		newProjectDescription: "",
	});

	return (
		<>
			<h3>Create New Project</h3>
			<div className="project-row" key={`project_row_new_project`}>
				<FontAwesomeIcon
					icon={faPlus}
					onClick={() => {
						if (projectCreationStates.newProjectName === "") {
							return;
						}

						const newProject: ProjectRequest = {
							name: projectCreationStates.newProjectName,
							description: projectCreationStates.newProjectDescription,
						};
						projectService
							.CreateNewProject(newProject)
							.then((createdProject) => {
								setProjectCreationStates(prevState => ({ ...prevState, newProjectName: "", newProjectDescription: "" }));
								let newProjectNameInput =
									document.getElementById(
										"newProjectName"
									) as HTMLInputElement | null;
								let newProjectDescriptionInput =
									document.getElementById(
										"newProjectDescripton"
									) as HTMLInputElement | null;
								if (newProjectNameInput) {
									newProjectNameInput.value = "";
								}
								if (newProjectDescriptionInput) {
									newProjectDescriptionInput.value = "";
								}
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
							})
							.catch((error: Error) => {
								console.error(error);
								setError(error.message);
							});
					}}
					className={
						projectCreationStates.newProjectName
							? "project-action-icon-new"
							: "project-action-icon-new-disabled"
					}
				/>
				<input
					type="text"
					id="newProjectName"
					onBlur={(e) => {
						setProjectCreationStates(prevState => ({ ...prevState, newProjectName: e.target.value }));
					}}
				/>
				<input
					type="text"
					id="newProjectDescription"
					onBlur={(e) => {
						setProjectCreationStates(prevState => ({ ...prevState, newProjectDescription: e.target.value }));
					}}
				/>
			</div>
		</>
	);
}

export default ProjectCreation;
