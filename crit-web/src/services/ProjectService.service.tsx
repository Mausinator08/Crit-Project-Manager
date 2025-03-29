import { GetEnvValues } from "../constants/environment";
import { Injectable } from "../functions/Dependencies/injectable";
import { Project } from "../models/project.model";
import { ProjectRequest } from "../models/requests/project-request.model";

@Injectable()
export class ProjectService {
	private readonly projectUrl: string =
		GetEnvValues()?.critApiUrl + "/Project";

	private projectIds: string[] = [];

	constructor() {}

	public GetProjectIds(): string[] {
		return this.projectIds;
	}

	public async GetAllProjects(): Promise<Project[]> {
		return new Promise<Project[]>(async (resolve, reject) => {
			const response = await fetch(new URL(this.projectUrl), {
				method: "GET",
				mode: "cors",
				credentials: "include",
				headers: {
					"Content-Type": "application/json",
				},
			});
			if (!response.ok) {
				reject(new Error(await response.text()));
				return;
			}
			const projects: Project[] = await response.json();
			this.projectIds = projects.map((p) => p.id!);
			resolve(projects);
		});
	}

	public GetProject(projectId: string): Promise<Project> {
		return new Promise<Project>(async (resolve, reject) => {
			const response = await fetch(
				new URL(`${this.projectUrl}/${projectId}`),
				{
					method: "GET",
					mode: "cors",
					credentials: "include",
					headers: {
						"Content-Type": "application/json",
					},
				}
			);
			if (!response.ok) {
				reject(new Error(await response.text()));
				return;
			}
			const project: Project = await response.json();
			resolve(project);
		});
	}

	public async CreateNewProject(project: ProjectRequest): Promise<Project> {
		return new Promise<Project>(async (resolve, reject) => {
			const response = await fetch(new URL(this.projectUrl), {
				method: "POST",
				mode: "cors",
				credentials: "include",
				headers: {
					"Content-Type": "application/json",
				},
				body: JSON.stringify(project),
			});
			if (!response.ok) {
				reject(new Error(await response.text()));
				return;
			}
			const newProject: Project = await response.json();
			resolve(newProject);
		});
	}

	public async UpdateProject(project: Project): Promise<void> {
		return new Promise<void>(async (resolve, reject) => {
			const response = await fetch(new URL(`${this.projectUrl}`), {
				method: "PUT",
				mode: "cors",
				credentials: "include",
				headers: {
					"Content-Type": "application/json",
				},
				body: JSON.stringify(project),
			});
			if (!response.ok) {
				reject(new Error(await response.text()));
				return;
			}
			resolve();
		});
	}

	public async DeleteProject(projectId: string): Promise<void> {
		return new Promise<void>(async (resolve, reject) => {
			const response = await fetch(
				new URL(`${this.projectUrl}/${projectId}`),
				{
					method: "DELETE",
					mode: "cors",
					credentials: "include",
					headers: {
						"Content-Type": "application/json",
					},
				}
			);
			if (!response.ok) {
				reject(new Error(await response.text()));
				return;
			}
			resolve();
		});
	}
}
